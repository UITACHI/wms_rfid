using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Common;

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
        public IStorageLocker Locker { get; set; } 

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortWorkDispatchService 成员

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "1":
                    statusStr = "未调度";
                    break;
                case "2":
                    statusStr = "调度中";
                    break;
                case "3":
                    statusStr = "已调度";
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
                DispatchStatus=WhatStatus(b.DispatchStatus),
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = (string)b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public new bool Add(SortWorkDispatch sortWorkDisp)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            Guid ID = new Guid(id);
            var sortOrderDispatch = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == ID);            
            if (sortOrderDispatch != null)
            {
                //解锁移库冻结量
                var moveDetail = MoveBillDetailRepository.GetQueryable().Where(m => m.BillNo == sortOrderDispatch.MoveBillNo);
                if (moveDetail.Count() > 0)
                {
                    foreach (var item in moveDetail.ToArray())
                    {
                        item.OutStorage.OutFrozenQuantity -= item.RealQuantity;
                        item.InStorage.InFrozenQuantity -= item.RealQuantity;
                    }
                }

                Del(MoveBillDetailRepository, sortOrderDispatch.MoveBillMaster.MoveBillDetails);//删除移库细单
                MoveBillMasterRepository.Delete(sortOrderDispatch.MoveBillMaster);//删除移库主单

                Del(OutBillDetailRepository, sortOrderDispatch.OutBillMaster.OutBillDetails);//删除出库细单
                OutBillMasterRepository.Delete(sortOrderDispatch.OutBillMaster);//删除出库主单
                //修改线路调度表中作业状态
                var sortDisp = SortOrderDispatchRepository.GetQueryable().Where(s => s.SortWorkDispatchID == sortOrderDispatch.ID);
                foreach (var item in sortDisp.ToArray())
                {
                    item.WorkStatus = "1";
                    item.SortWorkDispatchID = null;
                }               
                SortWorkDispatchRepository.Delete(sortOrderDispatch);
                SortWorkDispatchRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(SortWorkDispatch sortWorkDisp)
        {
            throw new NotImplementedException();
        }
        
        public bool Audit(string id, string userName)
        {
            bool result = false;
            Guid ID = new Guid(id);
            var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(i => i.ID == ID);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (employee != null)
            {
                if (sortWork != null && sortWork.DispatchStatus == "1")
                {
                    //出库审核
                    var outMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);                  
                    outMaster.Status = "2";
                    outMaster.VerifyPersonID = employee.ID;
                    outMaster.VerifyDate = DateTime.Now;
                    outMaster.UpdateTime = DateTime.Now;
                    //移库审核
                    var moveMater = MoveBillMasterRepository.GetQueryable().FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "2";
                    moveMater.VerifyPersonID = employee.ID;
                    moveMater.VerifyDate = DateTime.Now;
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业审核
                    sortWork.DispatchStatus = "2";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        public bool AntiTrial(string id)
        {
            bool result = false;
            Guid ID = new Guid(id);
            var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(i => i.ID == ID);
            if (sortWork != null && sortWork.DispatchStatus == "2")
            {
                //出库审核
                var outMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                outMaster.Status = "1";
                outMaster.UpdateTime = DateTime.Now;
                //移库审核
                var moveMater = MoveBillMasterRepository.GetQueryable().FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                moveMater.Status = "1";
                moveMater.UpdateTime = DateTime.Now;
                //分拣作业审核
                sortWork.DispatchStatus = "1";
                sortWork.UpdateTime = DateTime.Now;
                SortWorkDispatchRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        public bool Settle(string id, out string errorInfo)
        {
            bool result = false;
            Guid ID = new Guid(id);
            errorInfo = string.Empty;
            var sortWork = SortWorkDispatchRepository.GetQueryable().FirstOrDefault(i => i.ID == ID);
            //using (var scope = new TransactionScope())
            //{
            if (sortWork != null && sortWork.DispatchStatus != "4")
            {
                try
                {
                    //出库结单
                    var outMaster = OutBillMasterRepository.GetQueryable().FirstOrDefault(o => o.BillNo == sortWork.OutBillNo);
                    outMaster.Status = "7";
                    outMaster.UpdateTime = DateTime.Now;
                    //移库细单解锁冻结量
                    var moveDetail = MoveBillDetailRepository.GetQueryable().Where(m => m.BillNo == sortWork.MoveBillNo && m.Status != "2");
                    foreach (var item in moveDetail.ToArray())
                    {
                        if (Locker.LockNoEmptyStorage(item.OutStorage, item.Product) != null)//锁库存
                        {
                            item.OutStorage.OutFrozenQuantity -= item.RealQuantity;
                            item.OutStorage.LockTag = string.Empty;
                        }
                        else
                        {
                            errorInfo = "移出货位其他人员正在操作！无法结单！";
                            return false;
                        }
                        if (Locker.LockNoEmptyStorage(item.InStorage, item.Product) != null)//锁库存
                        {
                            item.InStorage.InFrozenQuantity -= item.RealQuantity;
                            item.InStorage.LockTag = string.Empty;
                        }
                        else
                        {
                            errorInfo = "移入货位其他人员正在操作！无法结单！";
                            return false;
                        }
                    }

                    //移库结单
                    var moveMater = MoveBillMasterRepository.GetQueryable().FirstOrDefault(m => m.BillNo == sortWork.MoveBillNo);
                    moveMater.Status = "5";
                    moveMater.UpdateTime = DateTime.Now;
                    //分拣作业结单
                    sortWork.DispatchStatus = "4";
                    sortWork.UpdateTime = DateTime.Now;
                    SortWorkDispatchRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    errorInfo = "结单失败！原因：" + e.Message;
                }
            }
            // scope.Complete();
            //   }
            return result;
        }

        #endregion
    }
}
