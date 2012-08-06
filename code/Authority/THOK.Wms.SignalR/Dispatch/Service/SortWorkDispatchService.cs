using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.SignalR.Connection;
using THOK.Wms.SignalR.Dispatch.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Wms.SignalR.Common;
using System.Transactions;

namespace THOK.Wms.SignalR.Dispatch.Service
{
    public class SortWorkDispatchService : Notifier<DispatchSortWorkConnection>, ISortOrderWorkDispatchService
    {
        [Dependency]
        public IStorageLocker Locker { get; set; }

        [Dependency]
        public ISortOrderDispatchRepository SortOrderDispatchRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public ISortOrderRepository SortOrderRepository { get; set; }
        [Dependency]
        public ISortOrderDetailRepository SortOrderDetailRepository { get; set; }
        [Dependency]
        public ISortingLowerlimitRepository SortingLowerlimitRepository { get; set; }
        [Dependency]
        public ISortingLineRepository SortingLineRepository { get; set; }
        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }

        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        [Dependency]
        public IMoveBillCreater MoveBillCreater { get; set; }
        [Dependency]
        public IOutBillCreater OutBillCreater { get; set; }

        public void Dispatch(string workDispatchId)
        {
            IQueryable<SortOrderDispatch> sortOrderDispatchQuery = SortOrderDispatchRepository.GetQueryable();
            IQueryable<SortOrder> sortOrderQuery = SortOrderRepository.GetQueryable();
            IQueryable<SortOrderDetail> sortOrderDetailQuery = SortOrderDetailRepository.GetQueryable();

            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();

            IQueryable<SortingLowerlimit> sortingLowerlimitQuery = SortingLowerlimitRepository.GetQueryable();
            IQueryable<SortingLine> sortingLineQuery = SortingLineRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();

            IQueryable<SortWorkDispatch> sortWorkDispatchQuery = SortWorkDispatchRepository.GetQueryable();

            workDispatchId = workDispatchId.Substring(0, workDispatchId.Length - 1);
            int[] work = workDispatchId.Split(',').Select(s => Convert.ToInt32(s)).ToArray();

            //调度表未作业的数据
            var temp = sortOrderDispatchQuery.Where(s => work.Any(w => w == s.ID) && s.WorkStatus == "1")
                                           .Join(sortOrderQuery,
                                                dp => new { dp.OrderDate, dp.DeliverLineCode },
                                                om => new { om.OrderDate, om.DeliverLineCode },
                                                (dp, om) => new { dp.OrderDate, dp.SortingLine, dp.DeliverLineCode, om.OrderID }
                                           ).Join(sortOrderDetailQuery,
                                                dm => new { dm.OrderID },
                                                od => new { od.OrderID },
                                                (dm, od) => new { dm.OrderDate, dm.SortingLine, od.Product, od.UnitCode, od.Price, od.RealQuantity }
                                           ).GroupBy(r => new { r.OrderDate, r.SortingLine, r.Product, r.UnitCode, r.Price })
                                            .Select(r => new { r.Key.OrderDate, r.Key.SortingLine, r.Key.Product, r.Key.UnitCode, r.Key.Price, SumQuantity = r.Sum(p => p.RealQuantity * r.Key.Product.UnitList.Unit02.Count) })
                                            .GroupBy(r => new { r.OrderDate, r.SortingLine })
                                            .Select(r => new { r.Key.OrderDate, r.Key.SortingLine, Products = r });

            using (var scope = new TransactionScope())
            {
                bool hasError = false;
                string strErrors = "";
                MoveBillMaster lastMoveBillMaster = null;
                foreach (var item in temp.ToArray())
                {
                    try
                    {
                        if (item.Products.Count() > 0)
                        {
                            if (lastMoveBillMaster != null && lastMoveBillMaster.WarehouseCode != item.SortingLine.Cell.WarehouseCode)
                            {
                                if (MoveBillCreater.CheckIsNeedSyncMoveBill(lastMoveBillMaster.WarehouseCode))
                                {
                                    MoveBillCreater.CreateSyncMoveBillDetail(lastMoveBillMaster);
                                }
                            }

                            //todo
                            MoveBillMaster moveBillMaster = MoveBillCreater.CreateMoveBillMaster(item.SortingLine.Cell.WarehouseCode, "3001", "操作员");
                            lastMoveBillMaster = moveBillMaster;
                            foreach (var product in item.Products.ToArray())
                            {
                                //获取分拣线下限数据
                                var sortingLowerlimitQuantity = sortingLowerlimitQuery.Where(s => s.ProductCode == product.Product.ProductCode
                                                                                                    && s.SortingLineCode == product.SortingLine.SortingLineCode)
                                                                                      .Sum(l => l.Quantity);
                                //获取分拣备货区库存                    
                                var storageQuantity = storageQuery.Where(s => s.ProductCode == product.Product.ProductCode)
                                                                  .Join(sortingLineQuery,
                                                                        s => s.Cell,
                                                                        l => l.Cell,
                                                                        (s, l) => new { l.SortingLineCode, s.Quantity }
                                                                  )
                                                                  .Where(r => r.SortingLineCode == product.SortingLine.SortingLineCode)
                                                                  .Sum(s => s.Quantity);
                                //获取移库量（按整件计）
                                decimal quantity = Math.Ceiling((product.SumQuantity + sortingLowerlimitQuantity - storageQuantity) / product.Product.Unit.Count)
                                                   * product.Product.Unit.Count;

                                AlltoMoveBill(moveBillMaster, product.Product, item.SortingLine.Cell, ref quantity);
                                if (quantity > 0)
                                {
                                    //生成移库不完整；
                                    hasError = true;
                                    strErrors = "生成移库不完整";
                                }
                            }

                            if (!hasError)
                            {
                                //todo
                                OutBillMaster outBillMaster = OutBillCreater.CreateOutBillMaster(item.SortingLine.Cell.WarehouseCode, "3001", "操作员");
                                //添加出库单细单
                                foreach (var product in item.Products.ToArray())
                                {
                                    OutBillCreater.AddToOutBillDetail(outBillMaster, product.Product, product.Price, product.SumQuantity);
                                }

                                //添加出库、移库主单和作业调度表
                                SortWorkDispatch sortWorkDisp = AddSortWorkDispMaster(moveBillMaster, outBillMaster, item.SortingLine.SortingLineCode, item.OrderDate);

                                //修改线路调度作业状态和作业ID
                                var sortDispTemp = sortOrderDispatchQuery.Where(s => work.Any(w => w == s.ID)
                                                                             && s.OrderDate == item.OrderDate
                                                                             && s.SortingLineCode == item.SortingLine.SortingLineCode);

                                foreach (var sortDisp in sortDispTemp.ToArray())
                                {
                                    sortDisp.SortWorkDispatchID = sortWorkDisp.ID;
                                    sortDisp.WorkStatus = "2";
                                    SortOrderDispatchRepository.SaveChanges();
                                }
                                scope.Complete();
                            }
                        }
                    }
                    catch (Exception)
                    {
                        strErrors =item.SortingLine.SortingLineName + "作业调度失败！";
                        return;
                    }
                }

                if (MoveBillCreater.CheckIsNeedSyncMoveBill(lastMoveBillMaster.WarehouseCode))
                {
                    MoveBillCreater.CreateSyncMoveBillDetail(lastMoveBillMaster);
                }
                scope.Complete();
            }
        }

        private SortWorkDispatch AddSortWorkDispMaster(MoveBillMaster moveBillMaster, OutBillMaster outBillMaster, string sortingLineCode, string orderDate)
        {
            //添加分拣作业调度表
            SortWorkDispatch sortWorkDispatch = new SortWorkDispatch();
            var workDispatch = SortWorkDispatchRepository.GetQueryable()
                                                         .FirstOrDefault(w => w.OrderDate == orderDate
                                                             && w.SortingLineCode == sortingLineCode);
            sortWorkDispatch.ID = Guid.NewGuid();
            sortWorkDispatch.OrderDate = orderDate;
            sortWorkDispatch.SortingLineCode = sortingLineCode;
            sortWorkDispatch.DispatchBatch = workDispatch == null ? "1" : (Convert.ToInt32(workDispatch.DispatchBatch) + 1).ToString();
            sortWorkDispatch.OutBillNo = outBillMaster.BillNo;
            sortWorkDispatch.MoveBillNo = moveBillMaster.BillNo;
            sortWorkDispatch.DispatchStatus = "2";
            sortWorkDispatch.IsActive = "1";
            sortWorkDispatch.UpdateTime = DateTime.Now;

            SortWorkDispatchRepository.Add(sortWorkDispatch);
            SortWorkDispatchRepository.SaveChanges();
            return sortWorkDispatch;
        }

        private void AlltoMoveBill(MoveBillMaster moveBillMaster, Product product,Cell cell,ref decimal quantity)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            //选择当前订单操作目标仓库；
            var storages = storageQuery.Where(s => s.Cell.WarehouseCode == moveBillMaster.WarehouseCode);
            storages = storages.Where(s => s.Quantity - s.OutFrozenQuantity > 0);

            //分配整盘；排除 件烟区 条烟区
            string[] areaTypes = new string[] { "2", "3" };
            var ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotPallet(moveBillMaster, ss, cell, ref quantity);

            //分配件烟；件烟区 
            areaTypes = new string[] { "2" };
            ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotPiece(moveBillMaster, ss, cell, ref quantity);

            //分配件烟 (下层储位)；排除 件烟区 条烟区 
            areaTypes = new string[] { "2", "3" };
            ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode
                                        && s.Cell.Layer == 1)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotPiece(moveBillMaster, ss, cell, ref quantity);

            //分配件烟 (非下层储位)；排除 件烟区 条烟区 
            areaTypes = new string[] { "2", "3" };
            ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode
                                        && s.Cell.Layer != 1)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotPiece(moveBillMaster, ss, cell, ref quantity);

            //分配条烟；条烟区
            areaTypes = new string[] { "3" };
            ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotBar(moveBillMaster, ss, cell, ref quantity);

            //分配条烟；件烟区
            areaTypes = new string[] { "2" };
            ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotBar(moveBillMaster, ss, cell, ref quantity);

            //分配条烟 (下层储位)；排除 件烟区 条烟区 
            areaTypes = new string[] { "2", "3" };
            ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode
                                        && s.Cell.Layer == 1)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotBar(moveBillMaster, ss, cell, ref quantity);

            //分配条烟 (非下层储位)；排除 件烟区 条烟区 
            areaTypes = new string[] { "2", "3" };
            ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                        && s.ProductCode == product.ProductCode
                                        && s.Cell.Layer != 1)
                             .OrderBy(s => s.StorageTime)
                             .OrderBy(s => s.Cell.Area.AllotOutOrder);
            AllotBar(moveBillMaster, ss, cell, ref quantity);
        }

        private void AllotBar(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> ss, Cell cell, ref decimal quantity)
        {
            foreach (var s in ss.ToArray())
            {
                if (quantity > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = quantity;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    if (allotQuantity > 0)
                    {
                        var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                        var targetStorage = Locker.LockEmpty(cell);
                        if (sourceStorage != null && targetStorage != null)
                        {
                            MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity);
                            quantity -= allotQuantity;
                        }
                        //解锁Storage；
                    }
                    else break;
                }
                else break;
            }
        }

        private void AllotPiece(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> ss, Cell cell, ref decimal quantity)
        {
            foreach (var s in ss.ToArray())
            {
                if (quantity > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = Math.Floor(quantity / s.Product.Unit.Count)
                                            * s.Product.Unit.Count;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    if (allotQuantity > 0)
                    {
                        var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                        var targetStorage = Locker.LockEmpty(cell);
                        if (sourceStorage != null && targetStorage != null)
                        {
                            MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity);
                            quantity -= allotQuantity;
                        }
                        //解锁Storage；
                    }
                    else break;
                }
                else break;
            }
        }

        private void AllotPallet(MoveBillMaster moveBillMaster, IOrderedQueryable<Storage> ss,Cell cell,ref decimal quantity)
        {
            foreach (var s in ss.ToArray())
            {
                if (quantity > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = Math.Floor(quantity / s.Product.Unit.Count)
                                            * s.Product.Unit.Count;
                    if (billQuantity >= allotQuantity)
                    {
                        var sourceStorage = Locker.LockNoEmptyStorage(s, s.Product);
                        var targetStorage = Locker.LockEmpty(cell);
                        if (sourceStorage != null && targetStorage != null)
                        {
                            MoveBillCreater.AddToMoveBillDetail(moveBillMaster, sourceStorage, targetStorage, allotQuantity);
                            quantity -= allotQuantity;
                        }
                        //解锁Storage；
                    }
                    else break;
                }
                else break;
            }
        }

    }
}
