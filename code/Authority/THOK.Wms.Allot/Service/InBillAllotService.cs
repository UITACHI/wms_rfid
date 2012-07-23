using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;

namespace THOK.Wms.Allot.Service
{
    public class InBillAllotService:ServiceBase<InBillAllot>,IInBillAllotService
    {
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }
        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public bool Allot(string billNo, string[] areaCodes)
        {
            IQueryable<InBillMaster> inBillMasterQuery = InBillMasterRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            InBillMaster billMaster = inBillMasterQuery.Single(b => b.BillNo == billNo);
            var cells = cellQuery.Where(c => c.WarehouseCode == billMaster.WarehouseCode //选择当前订单操作目标仓库；
                                    && (areaCodes == null || areaCodes.Any(a => a == c.AreaCode))//选择指定库区；
                                    && c.IsSingle == "1" //选择货位是单一存储的货位；
                                    && (c.Storage == null //选择未存储过的货位；
                                        || c.Storage.Any(s => s.IsLock != "1" //选择不是盘点状态的存位，
                                            && s.Quantity == 0                //并且库存量为0，
                                            && s.InFrozenQuantity == 0)));     //并且未入库量为0；
            var billDetails = billMaster.InBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0);//选择未分配的细单；

            var cellArray = cells.ToArray();
            var cellQueryForArray = cellArray.Where(c => c.Storage == null
                                                || c.Storage.Any(s => s.IsLock != "1" 
                                                    && s.Quantity == 0 
                                                    && s.InFrozenQuantity == 0))
                                             .OrderBy(c=>c.Area.AllotInOrder);

            foreach (var billDetail in billDetails.ToArray())
            {
                Cell cell;
                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配预设当前卷烟的货位；
                    cell = cellQueryForArray.Where(c => c.DefaultProductCode == billDetail.ProductCode)
                                                .First();
                    if (cell != null && LockCell(billNo,cell)) { Allot(billMaster, billDetail, cell); }
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配没预设卷烟的货位；
                    cell = cellQueryForArray.Where(c => c.DefaultProductCode == string.Empty)
                                            .First();
                    if (cell != null && LockCell(billNo,cell)) { Allot(billMaster, billDetail, cell); }
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配预设其他卷烟的货位；
                    cell = cellQueryForArray.Where(c => c.DefaultProductCode != billDetail.ProductCode 
                                               && c.DefaultProductCode != string.Empty)
                                            .First();
                    if (cell != null && LockCell(billNo,cell)) { Allot(billMaster, billDetail, cell); }
                }               

            }
            return true;
        }

        private bool LockCell(string billNo,Cell cell)
        {
            try
            {
                cell.IsSingle = "1";
                CellRepository.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void Allot(InBillMaster billMaster,InBillDetail billDetail, Cell cell)
        {
            if (LockStorage(billMaster.BillNo,cell))
            {
                InBillAllot billAllot = null;
                decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                                            / billDetail.Product.Unit.Count)
                                            * billDetail.Product.Unit.Count;
                allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;

                try
                {
                    billDetail.AllotQuantity += allotQuantity;
                    var storage = cell.Storage.Single();
                    storage.ProductCode = billDetail.ProductCode;
                    storage.LockTag = billDetail.BillNo;
                    storage.InFrozenQuantity += (int)allotQuantity;

                    billAllot = new InBillAllot()
                    {
                        BillNo = billMaster.BillNo,
                        ProductCode = billDetail.ProductCode,
                        CellCode = cell.CellCode,
                        StorageCode = storage.StorageCode,
                        UnitCode = billDetail.UnitCode,
                        AllotQuantity = allotQuantity,
                        RealQuantity = 0,
                        Status = "1"
                    };
                    billMaster.InBillAllots.Add(billAllot);
                    StorageRepository.SaveChanges();
                }
                catch (Exception)
                {
                    billDetail.AllotQuantity -= allotQuantity;
                    if (billMaster.InBillAllots.Contains(billAllot))
                    {
                        billMaster.InBillAllots.Remove(billAllot);
                    }
                }
            } 
        }

        private bool LockStorage(string billNo,Cell cell)
        {
            try
            {
                switch (cell.Storage.Count)
                {
                    case 0:
                        cell.Storage.Add(new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            LockTag = billNo,
                            IsActive = "0",
                            UpdateTime = DateTime.Now
                        });
                        StorageRepository.SaveChanges();
                        break;
                    case 1:
                        cell.Storage.Single().LockTag = Guid.NewGuid().ToString();
                        StorageRepository.SaveChanges();
                        break;
                    default:
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
