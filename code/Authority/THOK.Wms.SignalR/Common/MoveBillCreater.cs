using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.SignalR.Common
{
    public class MoveBillCreater : THOK.Wms.SignalR.Common.IMoveBillCreater
    {
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }

        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        public MoveBillMaster CreateMoveBillMaster(string warehouseCode, string billTypeCode, string operatePersonID)
        {
            //添加移库单主单
            string billNo = CreateMoveBillNo();            
            MoveBillMaster moveBillMaster = new MoveBillMaster();
            moveBillMaster.BillNo = billNo;
            moveBillMaster.BillDate = DateTime.Now;
            moveBillMaster.BillTypeCode = billTypeCode;
            moveBillMaster.WarehouseCode = warehouseCode;
            moveBillMaster.OperatePersonID = Guid.Parse(operatePersonID);
            moveBillMaster.Status = "1";
            moveBillMaster.Description = "出库生成同步移库单！";
            moveBillMaster.IsActive = "1";
            moveBillMaster.UpdateTime = DateTime.Now;
            MoveBillMasterRepository.Add(moveBillMaster);
            MoveBillMasterRepository.SaveChanges();
            return moveBillMaster;
        }

        public void CreateSyncMoveBillDetail(MoveBillMaster moveBillMaster)
        {
            Locker.LockKey = moveBillMaster.BillNo;

            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            var storages = storageQuery.Where(s => s.Cell.WarehouseCode == moveBillMaster.WarehouseCode
                                                   && s.Quantity - s.OutFrozenQuantity > 0
                                                   && s.OutFrozenQuantity > 0);

            var cells = cellQuery.Where(c => c.WarehouseCode == moveBillMaster.WarehouseCode);

            //1：主库区 1；2：件烟区 2；
            //3；条烟区 3；4：暂存区 4；
            //5：备货区 0；6：残烟区 0；
            //7：罚烟区 0；8：虚拟区 0；
            //9：其他区 0；

            //主库区未满盘件烟移到件烟区
            string[] areaTypes = new string[] { "1" };
            var ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                         && ((s.Quantity - s.OutFrozenQuantity) / s.Product.Unit.Count) > 1)
                             .ToArray();

            if (Locker.Lock(ss))
            {
                //件烟区 货位是单一存储的空货位； 
                areaTypes = new string[] { "2" };
                var cc = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                          && c.IsSingle == "1")
                              .ToArray();

                ss.AsParallel().ForAll(
                    (Action<Storage>)delegate(Storage s)
                    {
                        MoveToPieceArea(moveBillMaster, s, cc);
                    }
                );

                Locker.UnLock(ss);
            }
            else
                return;

            MoveBillDetailRepository.SaveChanges();

            //主库区件烟库区条烟移到条烟区
            areaTypes = new string[] { "1", "2" };
            ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                        && (s.Quantity - s.OutFrozenQuantity) % s.Product.Unit.Count > 0)
                         .ToArray();

            if (Locker.Lock(ss))
            {
                areaTypes = new string[] { "3" };
                var cc = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                                && c.IsSingle == "1")
                              .ToArray();

                ss.AsParallel().ForAll(
                    (Action<Storage>)delegate(Storage s)
                    {
                        MoveToBarArea(moveBillMaster, s,cc);
                    }
                );

                Locker.UnLock(ss);
            }
            else
                return;

            MoveBillDetailRepository.SaveChanges();
        }

        private void MoveToBarArea(MoveBillMaster moveBillMaster, Storage sourceStorage, Cell[] cc)
        {
            var cells = cc.Where(c => c.DefaultProductCode == sourceStorage.ProductCode
                                      || (c.Storages.Any()
                                         && c.Storages.FirstOrDefault().ProductCode == sourceStorage.ProductCode));
            foreach (var c in cells)
            {
                lock (c)
                {
                    var targetStorage = Locker.LockSingleArea(c);
                    if (sourceStorage != null && targetStorage != null
                        && (string.IsNullOrEmpty(targetStorage.ProductCode)
                            || targetStorage.ProductCode == sourceStorage.ProductCode
                            || (targetStorage.Quantity == 0
                                && targetStorage.InFrozenQuantity == 0)))
                    {
                        decimal moveQuantity = (sourceStorage.Quantity - sourceStorage.OutFrozenQuantity) % sourceStorage.Product.Unit.Count;
                        AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, moveQuantity);
                    }

                    if (targetStorage != null)
                    {
                        targetStorage.LockTag = string.Empty;
                    }
                }
            }

            cells = cc.Where(c => string.IsNullOrEmpty(c.DefaultProductCode));

            foreach (var c in cells)
            {
                lock (c)
                {
                    var targetStorage = Locker.LockSingleArea(c);
                    if (sourceStorage != null && targetStorage != null
                        && (string.IsNullOrEmpty(targetStorage.ProductCode)
                            || targetStorage.ProductCode == sourceStorage.ProductCode
                            || (targetStorage.Quantity == 0
                                && targetStorage.InFrozenQuantity == 0)))
                    {
                        decimal moveQuantity = (sourceStorage.Quantity - sourceStorage.OutFrozenQuantity) % sourceStorage.Product.Unit.Count;
                        AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, moveQuantity);
                    }
                    if (targetStorage != null)
                    {
                        targetStorage.LockTag = string.Empty;
                    }
                }
            }
        }

        private void MoveToPieceArea(MoveBillMaster moveBillMaster, Storage sourceStorage, Cell[] cc)
        {
            var cells = cc.Where(c => c.DefaultProductCode == sourceStorage.ProductCode
                                      || (c.Storages.Any()
                                         && c.Storages.FirstOrDefault().ProductCode == sourceStorage.ProductCode));
            foreach (var c in cells)
            {
                lock (c)
                {
                    var targetStorage = Locker.LockSingleArea(c);
                    if (sourceStorage != null && targetStorage != null
                        && (string.IsNullOrEmpty(targetStorage.ProductCode)
                            || targetStorage.ProductCode == sourceStorage.ProductCode
                            || (targetStorage.Quantity == 0 
                                && targetStorage.InFrozenQuantity == 0)))
                    {
                        decimal moveQuantity = Math.Floor((sourceStorage.Quantity - sourceStorage.OutFrozenQuantity) 
                                                    / sourceStorage.Product.Unit.Count)
                                                   * sourceStorage.Product.Unit.Count;
                        decimal targetAbleQuantity = Math.Floor((targetStorage.Cell.MaxQuantity 
                                                        * sourceStorage.Product.Unit.Count 
                                                        - targetStorage.Quantity 
                                                        - targetStorage.InFrozenQuantity
                                                        + targetStorage.OutFrozenQuantity) / sourceStorage.Product.Unit.Count)
                                                        * sourceStorage.Product.Unit.Count;
                        moveQuantity = moveQuantity <= targetAbleQuantity ? moveQuantity : targetAbleQuantity;
                        AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, moveQuantity);
                    }

                    if (targetStorage != null)
                    {
                        targetStorage.LockTag = string.Empty;
                    }
                }
            }

            cells = cc.Where(c => string.IsNullOrEmpty(c.DefaultProductCode));

            foreach (var c in cells)
            {
                lock (c)
                {
                    var targetStorage = Locker.LockSingleArea(c);
                    if (sourceStorage != null && targetStorage != null
                        && (string.IsNullOrEmpty(targetStorage.ProductCode)
                            || targetStorage.ProductCode == sourceStorage.ProductCode
                            || (targetStorage.Quantity == 0
                                && targetStorage.InFrozenQuantity == 0)))
                    {
                        decimal moveQuantity = Math.Floor((sourceStorage.Quantity - sourceStorage.OutFrozenQuantity)
                                                    / sourceStorage.Product.Unit.Count)
                                                   * sourceStorage.Product.Unit.Count;
                        decimal targetAbleQuantity = Math.Floor((c.MaxQuantity
                                                        * sourceStorage.Product.Unit.Count
                                                        - targetStorage.Quantity
                                                        - targetStorage.InFrozenQuantity
                                                        + targetStorage.OutFrozenQuantity)/sourceStorage.Product.Unit.Count)
                                                        * sourceStorage.Product.Unit.Count;
                        moveQuantity = moveQuantity <= targetAbleQuantity ? moveQuantity : targetAbleQuantity;
                        AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, moveQuantity);
                    }
                    if (targetStorage != null)
                    {
                        targetStorage.LockTag = string.Empty;
                    }
                }
            }
        }

        public bool CheckIsNeedSyncMoveBill(string warehouseCode)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.Where(s => s.Cell.WarehouseCode == warehouseCode
                                                   && s.Quantity - s.OutFrozenQuantity > 0
                                                   && s.OutFrozenQuantity > 0);

            //1：主库区 1；2：件烟区 2；
            //3；条烟区 3；4：暂存区 4；
            //5：备货区 0；6：残烟区 0；
            //7：罚烟区 0；8：虚拟区 0；
            //9：其他区 0；

            //主库区未满盘件烟移到件烟区
            string[] areaTypes = new string[] { "1" };
            var ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                            && ((s.Quantity - s.OutFrozenQuantity) / s.Product.Unit.Count) > 1);
            if (ss.Count() > 0 && storageQuery.Where(s=>s.Cell.Area.AreaType == "2").Count() > 0) { return true; }

            ////主库区件烟库区条烟移到条烟区
            //areaTypes = new string[] { "1", "2" };
            //ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
            //                            && (s.Quantity - s.OutFrozenQuantity) % s.Product.Unit.Count > 0);
            //if (ss.Count() > 0 && storageQuery.Where(s => s.Cell.Area.AreaType == "3").Count() > 0) { return true; }

            return false;
        }

        public void DeleteMoveBillDetail(MoveBillMaster moveBillMaster)
        {
            if (moveBillMaster != null)
            {
                foreach (var detail in moveBillMaster.MoveBillDetails)
                {
                    var sourceStorage = Locker.LockStorage(detail.OutStorage, detail.Product);
                    var targetStorage = Locker.LockStorage(detail.InStorage, detail.Product);
                    if (sourceStorage != null && targetStorage != null)
                    {
                        sourceStorage.OutFrozenQuantity -= detail.RealQuantity;
                        sourceStorage.LockTag = string.Empty;
                        targetStorage.InFrozenQuantity -= detail.RealQuantity;                        
                        targetStorage.LockTag = string.Empty;
                        detail.RealQuantity = 0;                        
                    }
                }
                var details = moveBillMaster.MoveBillDetails.Where(d => d.RealQuantity == 0)
                                                            .Select(d => d);
                MoveBillDetailRepository.Delete(details.ToArray());
                MoveBillDetailRepository.SaveChanges();
            }            
        }

        private void MoveToPieceArea(MoveBillMaster moveBillMaster, IQueryable<Storage> ss)
        {
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            //选择当前订单操作目标仓库；
            var cells = cellQuery.Where(c => c.WarehouseCode == moveBillMaster.WarehouseCode);
            //件烟区 货位是单一存储的空货位； 
            var areaTypes = new string[] { "2" };
            cells = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                            && c.IsSingle == "1"
                                        );
            foreach (var s in ss.ToArray())
            {
                var cc = cells.Where(c => c.DefaultProductCode == s.ProductCode
                                        || (c.Storages.Count() == 1
                                            && c.Storages.FirstOrDefault().ProductCode == s.ProductCode));
                foreach (var c in cc)
                {
                    var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                    var targetStorage = Locker.LockPiece(c, s.Product);
                    if (sourceStorage != null && targetStorage != null)
                    {
                        decimal moveQuantity = Math.Floor((sourceStorage.Quantity - sourceStorage.OutFrozenQuantity) / sourceStorage.Product.Unit.Count)
                                               * sourceStorage.Product.Unit.Count;
                        decimal targetAbleQuantity = targetStorage.Cell.MaxQuantity * sourceStorage.Product.Unit.Count - targetStorage.Quantity - targetStorage.InFrozenQuantity + targetStorage.OutFrozenQuantity;
                        moveQuantity = moveQuantity <= targetAbleQuantity ? moveQuantity : targetAbleQuantity;
                        AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, moveQuantity);
                    }
                }

                cc = cells.Where(c => string.IsNullOrEmpty(c.DefaultProductCode));
                foreach (var c in cc)
                {
                    var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                    var targetStorage = Locker.LockPiece(c, s.Product);
                    if (sourceStorage != null && targetStorage != null)
                    {                       
                        decimal moveQuantity = Math.Floor((sourceStorage.Quantity - sourceStorage.OutFrozenQuantity) / sourceStorage.Product.Unit.Count)
                                               * sourceStorage.Product.Unit.Count;
                        decimal targetAbleQuantity = targetStorage.Cell.MaxQuantity * sourceStorage.Product.Unit.Count - targetStorage.Quantity - targetStorage.InFrozenQuantity + targetStorage.OutFrozenQuantity;
                        moveQuantity = moveQuantity <= targetAbleQuantity ?moveQuantity:targetAbleQuantity;
                        AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, moveQuantity);
                    }
                }
            }
        }

        private void MoveToBarArea(MoveBillMaster moveBillMaster, IQueryable<Storage> ss)
        {
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            //选择当前订单操作目标仓库；
            var cells = cellQuery.Where(c => c.WarehouseCode == moveBillMaster.WarehouseCode);
            //条烟区 货位是单一存储的货位； 
            var areaTypes = new string[] { "3" };
            cells = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                            && c.IsSingle == "1"
                                        );
            foreach (var s in ss.ToArray())
            {
                var cc = cells.Where(c => c.DefaultProductCode == s.ProductCode
                        || (c.Storages.Count() == 1
                            && c.Storages.FirstOrDefault().ProductCode == s.ProductCode));
                if (cc.Count() > 0)
                {
                    foreach (var c in cc.ToArray())
                    {
                        var storage = Locker.LockNoEmptyStorage(s, s.Product);
                        if (storage != null)
                        {
                            decimal moveQuantity = (storage.Quantity - storage.OutFrozenQuantity) % storage.Product.Unit.Count;
                            AddToMoveBillDetail(moveBillMaster, storage, Locker.LockBar(c, s.Product), moveQuantity);
                        }
                    }
                }
                else
                {
                    cc = cells.Where(c => string.IsNullOrEmpty(c.DefaultProductCode));
                    foreach (var c in cc.ToArray())
                    {
                        var storage = Locker.LockNoEmptyStorage(s, s.Product);
                        if (storage != null)
                        {
                            decimal moveQuantity = (storage.Quantity - storage.OutFrozenQuantity) % storage.Product.Unit.Count;
                            AddToMoveBillDetail(moveBillMaster, storage, Locker.LockBar(c, s.Product), moveQuantity);
                        }
                    }
                }
            }
        }

        public void AddToMoveBillDetail(MoveBillMaster moveBillMaster, Storage sourceStorage, Storage targetStorage, decimal moveQuantity)
        {
            if (moveQuantity > 0)
            {
                Locker.LockKey = moveBillMaster.BillNo;
                MoveBillDetail detail = new MoveBillDetail();
                detail.BillNo = moveBillMaster.BillNo;
                detail.ProductCode = sourceStorage.ProductCode;
                detail.OutCellCode = sourceStorage.CellCode;
                detail.OutStorageCode = sourceStorage.StorageCode;
                detail.InCellCode = targetStorage.CellCode;
                detail.InStorageCode = targetStorage.StorageCode;
                detail.UnitCode = sourceStorage.Product.UnitCode;
                detail.RealQuantity = moveQuantity;
                detail.Status = "0";
                moveBillMaster.MoveBillDetails.Add(detail);

                sourceStorage.OutFrozenQuantity += moveQuantity;
                sourceStorage.LockTag = string.Empty;
                targetStorage.ProductCode = sourceStorage.ProductCode;
                targetStorage.InFrozenQuantity += moveQuantity;
                targetStorage.LockTag = string.Empty;
            }
        }

        public string CreateMoveBillNo()
        {
            string billNo = "";
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");            
            var billNos = moveBillMasterQuery.Where(i => i.BillNo.Contains(sysTime))
                                                  .AsEnumerable()
                                                  .OrderBy(i => i.BillNo)
                                                  .Select(i => i.BillNo);
            if (billNos.Count() == 0)
            {
                billNo = System.DateTime.Now.ToString("yyMMdd") + "0001" + "MO";
            }
            else
            {
                string billNoStr = billNos.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                billNo = System.DateTime.Now.ToString("yyMMdd") + newcode + "MO";
            }

            return billNo;
        }
    }
}
