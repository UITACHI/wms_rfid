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
            var billDetails = billMaster.InBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0);//选择未分配的细单；

            var cells = cellQuery.Where(c => c.WarehouseCode == billMaster.WarehouseCode //选择当前订单操作目标仓库；
                                    && (areaCodes == null || areaCodes.Any(a => a == c.AreaCode)));//选择指定库区；
                                    
                                    //&& c.IsSingle == "1" //选择货位是单一存储的货位；                                 
                                    //&& c.Area.AllotInOrder > 0
                                    //&& (c.Storage == null //选择未存储过的货位；
                                    //    || c.Storage.Any(s => (s.LockTag == null || s.LockTag == string.Empty)  //选择不是被锁定状态的货位，
                                    //        && s.Quantity == 0                //并且库存量为0，
                                    //        && s.InFrozenQuantity == 0)));     //并且未入库量为0；
            
            //排除 条烟区，件烟区
            string [] areaTypes = new string []{};
            var cellList1 = cells.Where(c => areaTypes.All(a => a != c.Area.AreaType)
                                            && c.Area.AllotInOrder > 0
                                            && (c.Storage.Count==0
                                                    || c.Storage.Any(s => (s.LockTag == null 
                                                                        || s.LockTag == string.Empty)
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                        ).ToList();

            //条烟区
            areaTypes = new string[] {};
            var cellList2 = cells.Where(c => areaTypes.All(a => a != c.Area.AreaType)).ToList();

            //件烟区
            areaTypes = new string[] {};
            var cellList3 = cells.Where(c => areaTypes.All(a => a != c.Area.AreaType)
                                            && c.Area.AllotInOrder > 0
                                            && (c.Storage.Count == 0
                                                    || c.Storage.Any(s => (s.LockTag == null || s.LockTag == string.Empty)
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                        ).ToList();

            //排除 条烟区，件烟区
            var cellQueryFromList1 = cellList1.Where(c => c.Storage.Count == 0
                                                || c.Storage.Any(s => (s.LockTag == null || s.LockTag == string.Empty) 
                                                    && s.Quantity == 0 
                                                    && s.InFrozenQuantity == 0))
                                             .OrderBy(c=>c.Area.AllotInOrder);
            //条烟区
            var cellQueryFromList2 = cellList2.OrderBy(c => c.Area.AllotInOrder);

            //件烟区
            var cellQueryFromList3 = cellList3.Where(c => c.Storage.Count == 0
                                                || c.Storage.Any(s => (s.LockTag == null || s.LockTag == string.Empty
                                                    && s.Quantity == 0
                                                    && s.InFrozenQuantity == 0)))
                                             .OrderBy(c => c.Area.AllotInOrder);

            foreach (var billDetail in billDetails.ToArray())
            {
                Cell cell;
                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配预设当前卷烟的货位；
                    cell = cellQueryFromList1.Where(c => c.DefaultProductCode == billDetail.ProductCode)
                                            .FirstOrDefault();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                            / billDetail.Product.Unit.Count)
                            * billDetail.Product.Unit.Count;
                        if (billQuantity >= allotQuantity)
                        {
                            if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                            else cellList1.Remove(cell);
                        }
                        else break;
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配没预设卷烟的货位；
                    cell = cellQueryFromList1.Where(c => c.DefaultProductCode == string.Empty)
                                             .FirstOrDefault();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                            / billDetail.Product.Unit.Count)
                            * billDetail.Product.Unit.Count;
                        if (billQuantity >= allotQuantity)
                        {
                            if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                            else cellList1.Remove(cell);
                        }
                        else break;
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配预设其他卷烟的货位；
                    cell = cellQueryFromList1.Where(c => c.DefaultProductCode != billDetail.ProductCode
                                                        && c.DefaultProductCode != string.Empty)
                                             .FirstOrDefault();
                    if (cell != null )
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                            / billDetail.Product.Unit.Count)
                            * billDetail.Product.Unit.Count;
                        if (billQuantity >= allotQuantity)
                        {
                            if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                            else cellList1.Remove(cell);
                        }
                        else break;
                    }
                    else break;                       
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配条烟到条烟区；
                    cell = cellQueryFromList2.FirstOrDefault();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                            / billDetail.Product.Unit.Count)
                            * billDetail.Product.Unit.Count;
                        if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                        else cellList1.Remove(cell);
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配未满一托盘的卷烟到件烟区；
                    cell = cellQueryFromList3.FirstOrDefault();
                    if (cell != null)
                    {
                        if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                        else cellList1.Remove(cell);
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配未满一托盘的卷烟到下层货架；
                    cell = cellQueryFromList1.Where(c => c.Layer == 1)
                                            .FirstOrDefault();
                    if (cell != null)
                    {
                        if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                        else cellList1.Remove(cell);
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配未分配卷烟到其他库区；
                    cell = cellQueryFromList1.FirstOrDefault();
                    if (cell != null)
                    {
                        if (LockCell(billNo, cell)) { Allot(billMaster, billDetail, cell); }
                        else cellList1.Remove(cell);
                    }
                    else break;
                }
            }
            return true;
        }

        private bool LockCell(string billNo,Cell cell)
        {
            try
            {
                cell.LockTag = billNo;
                CellRepository.SaveChanges();                
            }
            catch (Exception)
            {
                CellRepository.Detach(cell);
                return false;
            }

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
                        break;
                    case 1:
                        cell.Storage.Single().LockTag = billNo;
                        break;
                    default:
                        return false;
                }                
                StorageRepository.SaveChanges();
            }
            catch (Exception)
            {
                StorageRepository.Detach(cell.Storage.Single());
                return false;
            }

            return true;
        }

        private void Allot(InBillMaster billMaster, InBillDetail billDetail, Cell cell)
        {
            InBillAllot billAllot = null;
            decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
            decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                                        / billDetail.Product.Unit.Count)
                                        * billDetail.Product.Unit.Count;
            allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;

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
    }
}
