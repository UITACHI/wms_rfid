using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;

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

        public bool Allot(string billNo, string[] areaCodes,out string result)
        {
            result = string.Empty;
            IQueryable<InBillMaster> inBillMasterQuery = InBillMasterRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            InBillMaster billMaster = inBillMasterQuery.Single(b => b.BillNo == billNo);
            var billDetails = billMaster.InBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0);//选择未分配的细单；

            var cells = cellQuery.Where(c => c.WarehouseCode == billMaster.WarehouseCode); //选择当前订单操作目标仓库；
            if (areaCodes.Length > 0)
            {
                cells = cells.Where(c => areaCodes.Any(a => a == c.AreaCode));//选择指定库区；
            }
            else
            {
                cells = cells.Where(c => c.Area.AllotInOrder > 0);
            }
            
            //1：主库区；2：件烟区；
            //3；条烟区；4：暂存区；
            //5：备货区；6：残烟区；
            //7：罚烟区；8：虚拟区；
            //9：其他区；

            //排除 件烟区,条烟区
            string [] areaTypes = new string []{"2","3"};
            var cellList1 = cells.Where(c => areaTypes.All(a => a != c.Area.AreaType)
                                            && c.IsSingle == "1" //选择货位是单一存储的货位；     
                                            && (c.Storages.Count==0
                                                    || c.Storages.Any(s => (s.LockTag == null 
                                                                        || s.LockTag == string.Empty)
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                        ).ToList();

            //条烟区
            areaTypes = new string[] {"3"};
            var cellList2 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                            && c.IsSingle == "1" //选择货位是单一存储的货位；     
                                        ).ToList();

            //件烟区
            areaTypes = new string[] {"2"};
            var cellList3 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                            && c.IsSingle == "1" //选择货位是单一存储的货位；     
                                            && (c.Storages.Count == 0
                                                    || c.Storages.Any(s => (s.LockTag == null || s.LockTag == string.Empty)
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                        ).ToList();

            //非货位管理区
            List<Cell> cellList4 = new List<Cell>();
            cellList4 = cells.Where(c => c.IsSingle == "0").ToList();


            //排除 件烟区，条烟区
            var cellQueryFromList1 = cellList1.Where(c => c.Storages.Count == 0
                                                || c.Storages.Any(s => (s.LockTag == null || s.LockTag == string.Empty) 
                                                    && s.Quantity == 0 
                                                    && s.InFrozenQuantity == 0))
                                             .OrderBy(c=>c.Area.AllotInOrder);
            //条烟区
            var cellQueryFromList2 = cellList2.OrderBy(c => c.Area.AllotInOrder);

            //件烟区
            var cellQueryFromList3 = cellList3.Where(c => c.Storages.Count == 0
                                                || c.Storages.Any(s => (s.LockTag == null || s.LockTag == string.Empty
                                                    && s.Quantity == 0
                                                    && s.InFrozenQuantity == 0)))
                                             .OrderBy(c => c.Area.AllotInOrder);
            //非货位管理区
            var cellQueryFromList4 = cellList4.OrderBy(c => c.Area.AllotInOrder);

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
                            Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity);
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
                            Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity); 
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
                            Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity); 
                        }
                        else break;
                    }
                    else break;                       
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配条烟到条烟区；todo
                    cell = cellQueryFromList2.FirstOrDefault();
                    if (cell != null)
                    {                        
                        decimal billQuantity = (billDetail.BillQuantity - billDetail.AllotQuantity) % billDetail.Product.Unit.Count;
                        if (billQuantity > 0)
                        {
                            Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), billQuantity);
                        }
                        else break;
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配未满一托盘的卷烟到件烟区；
                    cell = cellQueryFromList3.FirstOrDefault();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                        allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                        Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity); 
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
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                        allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                        Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity); 
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配未分配卷烟到其他库区；
                    cell = cellQueryFromList1.FirstOrDefault();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                            / billDetail.Product.Unit.Count)
                            * billDetail.Product.Unit.Count;
                        allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                        Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity); 
                    }
                    else break;
                }

                while ((billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    //分配未分配卷烟到其他非货位管理货位；
                    cell = cellQueryFromList4.FirstOrDefault();
                    if (cell != null)
                    {
                        decimal allotQuantity = cell.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                        allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                        Allot(billMaster, billDetail, cell, LockStorage(billNo, cell), allotQuantity);
                    }
                    else break;
                }
            }

            if (billMaster.InBillDetails.Any(i=>i.BillQuantity - i.AllotQuantity > 0))
            {
                result = "分配未全部完成，没有储位可分配！";
            }
            return result == string.Empty;
        }

        private Storage LockStorage(string billNo, Cell cell)
        {
            try
            {
                cell.LockTag = billNo;
                CellRepository.SaveChanges();                
            }
            catch (Exception)
            {
                CellRepository.Detach(cell);
                return null;
            }

            Storage storage = null;
            try
            {                
                if (cell.IsSingle == "1")
                {
                    if (cell.Storages.Count == 0)
                    {
                        storage = new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            LockTag = billNo,
                            IsActive = "0",
                            StorageTime =  DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        cell.Storages.Add(storage);
                    }
                    else if (cell.Storages.Count == 1)
                    {
                        storage = cell.Storages.Single();
                        storage.LockTag = billNo;
                    }
                }
                else
                {
                    storage = cell.Storages.Where(s => s.LockTag == null || s.LockTag == string.Empty
                                                && s.Quantity == 0
                                                && s.InFrozenQuantity == 0)
                                          .FirstOrDefault();
                    if (storage != null)
                    {
                        storage.LockTag = billNo;
                    }
                    else
                    {
                        storage = new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            LockTag = billNo,
                            IsActive = "0",
                            StorageTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        cell.Storages.Add(storage);
                    }
                }
                StorageRepository.SaveChanges();
            }
            catch (Exception)
            {
                StorageRepository.Detach(storage);
                cell.Storages.Remove(storage);
                storage = null;
            }

            cell.LockTag = string.Empty;
            CellRepository.SaveChanges();  

            return storage;
        }

        private void Allot(InBillMaster billMaster, InBillDetail billDetail, Cell cell, Storage storage, decimal allotQuantity)
        {
            if (storage != null)
            {
                InBillAllot billAllot = null;
                billDetail.AllotQuantity += allotQuantity;                
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
}
