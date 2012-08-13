using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Common;
using System.Transactions;

namespace THOK.Wms.Bll.Service
{
    public class SortWorkDispatchService : ServiceBase<SortWorkDispatch>, ISortWorkDispatchService
    {
        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public ISortOrderDispatchRepository SortOrderDispatchRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "1":
                    statusStr = "已调度";
                    break;
                case "2":
                    statusStr = "已审核";
                    break;
                case "3":
                    statusStr = "执行中";
                    break;
                case "4":
                    statusStr = "已结单";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string OrderDate, string SortingLineCode, string DispatchStatus)
        {
            IQueryable<SortWorkDispatch> SortWorkDispatchQuery = SortWorkDispatchRepository.GetQueryable();
            var sortWorkDispatch = SortWorkDispatchQuery.Where(s => s.SortingLineCode == s.SortingLineCode);
            if (OrderDate != string.Empty && OrderDate != null)
            {
                OrderDate = Convert.ToDateTime(OrderDate).ToString("yyyyMMdd");
                sortWorkDispatch = sortWorkDispatch.Where(s => s.OrderDate == OrderDate);
            }
            if (SortingLineCode != string.Empty && SortingLineCode != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.SortingLineCode == SortingLineCode);
            }
            if (DispatchStatus != string.Empty && DispatchStatus != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.DispatchStatus == DispatchStatus);
            }
            var temp = sortWorkDispatch.OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.ID,
                b.OrderDate,
                b.SortingLineCode,
                b.SortingLine.SortingLineName,
                b.OutBillNo,
                b.MoveBillNo,
                b.DispatchBatch,
                DispatchStatus = WhatStatus(b.DispatchStatus),
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = (string)b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public bool Delete(string id, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortOrderDispatch = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);
                if (sortOrderDispatch == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能删除！";
                    return false;
                }
                if (sortOrderDispatch.DispatchStatus != "1")
                {
                    errorInfo = "当前选择的调度记录不是已调度，未能删除！";
                    return false;
                }
                if (sortOrderDispatch.OutBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录出库单不是已录入，未能删除！";
                    return false;
                }
                if (sortOrderDispatch.MoveBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录移库单不是已录入，未能删除！";
                    return false;
                }

                using (var scope = new TransactionScope())
                {
                    //解锁移库冻结量
                    var moveDetail = MoveBillDetailRepository.GetQueryable()
                                                                .Where(m => m.BillNo
                                                                    == sortOrderDispatch.MoveBillNo);

                    var sourceStorages = moveDetail.Select(m => m.OutStorage).ToArray();
                    var targetStorages = moveDetail.Select(m => m.InStorage).ToArray();

                    if (!Locker.Lock(sourceStorages)
                        || !Locker.Lock(targetStorages))
                    {
                        errorInfo = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                        return false;
                    }

                    moveDetail.AsParallel().ForAll(
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
                                throw new Exception("储位的卷烟或移库冻结量与当前分配不符，信息可能被异常修改，不能结单！");
                            }
                        }
                    );

                    Del(MoveBillDetailRepository, sortOrderDispatch.MoveBillMaster.MoveBillDetails);//删除移库细单
                    MoveBillMasterRepository.Delete(sortOrderDispatch.MoveBillMaster);//删除移库主单

                    Del(OutBillDetailRepository, sortOrderDispatch.OutBillMaster.OutBillDetails);//删除出库细单
                    OutBillMasterRepository.Delete(sortOrderDispatch.OutBillMaster);//删除出库主单

                    //修改线路调度表中作业状态
                    var sortDisp = SortOrderDispatchRepository.GetQueryable()
                                                              .Where(s => s.SortWorkDispatchID
                                                                  == sortOrderDispatch.ID);
                    foreach (var item in sortDisp.ToArray())
                    {
                        item.WorkStatus = "1";
                        item.SortWorkDispatchID = null;
                    }
                    SortWorkDispatchRepository.Delete(sortOrderDispatch);
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "删除失败，详情：" + e.Message;
                return false;
            }
        }

        public bool Audit(string id, string userName, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);
                var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);

                if (employee == null)
                {
                    errorInfo = "当前用户不存在或不可用，未能审核！";
                    return false;
                }
                if (sortWork == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能审核！";
                    return false;
                }
                if (sortWork.DispatchStatus != "1")
                {
                    errorInfo = "当前选择的调度记录不是已调度，未能审核！";
                    return false;
                }
                if (sortWork.OutBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录出库单不是已录入，未能审核！";
                    return false;
                }
                if (sortWork.MoveBillMaster.Status != "1")
                {
                    errorInfo = "当前选择的调度记录移库单不是已录入，未能审核！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    //出库审核
                    var outMaster = OutBillMasterRepository.GetQueryable()
                        .FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "2";
                    outMaster.VerifyPersonID = employee.ID;
                    outMaster.VerifyDate = DateTime.Now;
                    outMaster.UpdateTime = DateTime.Now;
                    //移库审核
                    var moveMater = MoveBillMasterRepository.GetQueryable()
                        .FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "2";
                    moveMater.VerifyPersonID = employee.ID;
                    moveMater.VerifyDate = DateTime.Now;
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业审核
                    sortWork.DispatchStatus = "2";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "审核失败，详情：" + e.Message;
                return false;
            }
        }

        public bool AntiTrial(string id, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortWork = SortWorkDispatchRepository.GetQueryable()
                                                         .FirstOrDefault(s => s.ID == ID);

                if (sortWork == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能反审！";
                    return false;
                }
                if (sortWork.DispatchStatus != "2")
                {
                    errorInfo = "当前选择的调度记录不是已审核，未能反审！";
                    return false;
                }
                if (sortWork.OutBillMaster.Status != "2")
                {
                    errorInfo = "当前选择的调度记录出库单不是已审核，未能反审！";
                    return false;
                }
                if (sortWork.MoveBillMaster.Status != "2")
                {
                    errorInfo = "当前选择的调度记录移库单不是已审核，未能反审！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    //出库反审
                    var outMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "1";
                    outMaster.UpdateTime = DateTime.Now;
                    //移库反审
                    var moveMater = MoveBillMasterRepository.GetQueryable().FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "1";
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业反审
                    sortWork.DispatchStatus = "1";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (Exception e)
            {
                errorInfo = "反审失败，详情：" + e.Message;
                return false;
            }
        }

        public bool Settle(string id, ref string errorInfo)
        {
            try
            {
                Guid ID = new Guid(id);
                var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);

                if (sortWork == null)
                {
                    errorInfo = "当前选择的调度记录不存在，未能结单！";
                    return false;
                }
                if (sortWork.DispatchStatus != "3")
                {
                    errorInfo = "当前选择的调度记录不是执行中，未能结单！";
                    return false;
                }
                if (sortWork.MoveBillMaster.Status != "3")
                {
                    errorInfo = "当前选择的调度记录移库单不是执行中，未能结单！";
                    return false;
                }
                using (var scope = new TransactionScope())
                {
                    //移库细单解锁冻结量
                    var moveDetail = MoveBillDetailRepository.GetQueryable()
                        .Where(m => m.BillNo == sortWork.MoveBillNo && m.Status != "2");

                    if (moveDetail.Any())
                    {
                        var sourceStorages = moveDetail.Select(m => m.OutStorage).ToArray();
                        var targetStorages = moveDetail.Select(m => m.InStorage).ToArray();

                        if (!Locker.Lock(sourceStorages)
                            || !Locker.Lock(targetStorages))
                        {
                            errorInfo = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                            return false;
                        }

                        moveDetail.AsParallel().ForAll(
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
                                    throw new Exception("储位的卷烟或移库冻结量与当前分配不符，信息可能被异常修改，不能结单！");
                                }
                            }
                        );
                        //解锁分拣线路调度的状态，以便重新做作业调度；
                        foreach (var sortDisp in sortWork.SortOrderDispatchs)
                        {
                            sortDisp.SortWorkDispatchID = null;
                            sortDisp.WorkStatus = "1";
                        }
                    }
                    else
                    {
                        //出库单作自动出库
                        var storages = StorageRepository.GetQueryable().Where(s => s.CellCode == sortWork.SortingLine.CellCode
                                                                                && s.Quantity - s.OutFrozenQuantity >0).ToArray();

                        if (!Locker.Lock(storages))
                        {
                            errorInfo = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                            return false;
                        }

                        sortWork.OutBillMaster.OutBillDetails.AsParallel().ForAll(
                            (Action<OutBillDetail>)delegate(OutBillDetail o)
                            {
                                var ss = storages.Where(s => s.ProductCode == o.ProductCode).ToArray();
                                foreach (var s in ss)
                                {
                                    lock (s)
                                    {
                                        if (o.BillQuantity - o.AllotQuantity > 0)
                                        {
                                            decimal allotQuantity = s.Quantity;
                                            decimal billQuantity = o.BillQuantity - o.AllotQuantity;
                                            allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                                            o.AllotQuantity += allotQuantity;
                                            o.RealQuantity += allotQuantity;
                                            s.Quantity -= allotQuantity;

                                            var billAllot = new OutBillAllot()
                                            {
                                                BillNo = sortWork.OutBillMaster.BillNo,
                                                OutBillDetailId = o.ID,
                                                ProductCode = o.ProductCode,
                                                CellCode = s.CellCode,
                                                StorageCode = s.StorageCode,
                                                UnitCode = o.UnitCode,
                                                AllotQuantity = allotQuantity,
                                                RealQuantity = allotQuantity,
                                                Status = "2"
                                            };
                                            lock (sortWork.OutBillMaster.OutBillAllots)
                                            {
                                                sortWork.OutBillMaster.OutBillAllots.Add(billAllot);
                                            }
                                        }
                                        else
                                            break;
                                    }
                                }

                                if (o.BillQuantity - o.AllotQuantity > 0)
                                {
                                    throw new Exception(sortWork.SortingLine.SortingLineName + " " + o.ProductCode + " " + o.Product.ProductName + "分拣备货区库存不足，未能结单！");
                                }
                            }
                        );

                        storages.AsParallel().ForAll(s=>s.LockTag = string.Empty);
                    }                   

                    //出库结单
                    var outMaster = OutBillMasterRepository.GetQueryable()
                        .FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "6";
                    outMaster.UpdateTime = DateTime.Now;
                    //移库结单
                    var moveMater = MoveBillMasterRepository.GetQueryable()
                        .FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "4";
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业结单
                    sortWork.DispatchStatus = "4";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    scope.Complete();
                    return true;
                }
            }
            catch (AggregateException ex)
            {
                errorInfo = "结单失败，详情：" + ex.InnerExceptions.Select(i => i.Message).Aggregate((m, n) => m + n);
                return false;
            }
            catch (Exception e)
            {
                errorInfo = "结单失败，详情：" + e.Message;
                return false;
            }
        }
    }
}
