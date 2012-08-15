using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using Entities.Extensions;

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
            moveBillMaster.Origin = "1";
            moveBillMaster.WarehouseCode = warehouseCode;
            moveBillMaster.OperatePersonID = Guid.Parse(operatePersonID);
            moveBillMaster.Status = "1";            
            moveBillMaster.IsActive = "1";
            moveBillMaster.UpdateTime = DateTime.Now;
            MoveBillMasterRepository.Add(moveBillMaster);
            return moveBillMaster;
        }

        public void CreateSyncMoveBillDetail(MoveBillMaster moveBillMaster)
        {
            Locker.LockKey = moveBillMaster.BillNo;

            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryableIncludeStorages();

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
                    var targetStorage = Locker.LockStorage(c);
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
                    var targetStorage = Locker.LockStorage(c);
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
                    var targetStorage = Locker.LockStorage(c);
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
                    var targetStorage = Locker.LockStorage(c);
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
                var sourceStorages = moveBillMaster.MoveBillDetails.Select(m => m.OutStorage).ToArray();
                var targetStorages = moveBillMaster.MoveBillDetails.Select(m => m.InStorage).ToArray();

                if (Locker.Lock(sourceStorages) && Locker.Lock(targetStorages))
                {
                    moveBillMaster.MoveBillDetails.AsParallel().ForAll(
                        (Action<MoveBillDetail>)delegate(MoveBillDetail m)
                        {
                            if (m.InStorage.ProductCode == m.ProductCode
                                && m.OutStorage.ProductCode == m.ProductCode
                                && m.InStorage.InFrozenQuantity >= m.RealQuantity
                                && m.OutStorage.OutFrozenQuantity >= m.RealQuantity)
                            {
                                m.InStorage.InFrozenQuantity -= m.RealQuantity;
                                m.OutStorage.OutFrozenQuantity -= m.RealQuantity;
                                m.InStorage.LockTag = string.Empty;
                                m.OutStorage.LockTag = string.Empty;
                            }
                            else
                            {
                                throw new Exception("储位的卷烟或入库冻结量与当前分配不符，信息可能被异常修改，不能删除移库单！");
                            }
                        }
                    );

                    Locker.UnLock(sourceStorages);
                    Locker.UnLock(targetStorages);
                }
                else
                    throw new Exception("锁定储位失败，其他人可能正在操作，请稍候重试!");

                MoveBillDetailRepository.Delete(moveBillMaster.MoveBillDetails.ToArray());
                MoveBillDetailRepository.GetObjectSet()
                    .DeleteEntity(m => m.BillNo == moveBillMaster.BillNo);
                                        
                MoveBillDetailRepository.SaveChanges();
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
                targetStorage.ProductCode = sourceStorage.ProductCode;
                targetStorage.InFrozenQuantity += moveQuantity;
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
