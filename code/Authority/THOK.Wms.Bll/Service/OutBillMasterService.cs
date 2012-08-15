using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Common;
using System.Transactions;

namespace THOK.Wms.Bll.Service
{
    public class OutBillMasterService : ServiceBase<OutBillMaster>, IOutBillMasterService
    {
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }

        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }

        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }

        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }

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
                    statusStr = "已录入";
                    break;
                case "2":
                    statusStr = "已审核";
                    break;
                case "3":
                    statusStr = "已分配";
                    break;
                case "4":
                    statusStr = "已确认";
                    break;
                case "5":
                    statusStr = "执行中";
                    break;
                case "6":
                    statusStr = "已结单";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<OutBillMaster> OutBillMasterQuery = OutBillMasterRepository.GetQueryable().Where(o => o.Status != "6");

            var outBillMaster = OutBillMasterQuery.AsEnumerable()
                                    .OrderByDescending(t => t.BillDate)
                                    .OrderByDescending(t => t.BillNo)
                                    .Select(i =>i);
            if (!BillNo.Equals(string.Empty) && BillNo != null)
            {
                outBillMaster = outBillMaster.Where(i => i.BillNo.Contains(BillNo));
            }
            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                outBillMaster = outBillMaster.Where(i => Convert.ToDateTime(i.BillDate) >= begin);
            }
            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate).AddDays(1);
                outBillMaster = outBillMaster.Where(i => Convert.ToDateTime(i.BillDate) <= end);
            }
            if (!OperatePersonCode.Equals(string.Empty) && OperatePersonCode != null)
            {
                outBillMaster = outBillMaster.Where(i => i.OperatePerson.EmployeeCode.Contains(OperatePersonCode));
            }
            if (!Status.Equals(string.Empty))
            {
                outBillMaster = outBillMaster.Where(i => i.Status.Contains(Status) && i.Status != "6");
            }
            if (!IsActive.Equals(string.Empty))
            {
                outBillMaster = outBillMaster.Where(i => i.IsActive.Contains(IsActive));
            }
            int total = outBillMaster.Count();
            outBillMaster = outBillMaster.Skip((page - 1) * rows).Take(rows);

            var temp = outBillMaster.ToArray().AsEnumerable().Select(i => new
                    {
                        i.BillNo,
                        BillDate = i.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        i.Warehouse.WarehouseCode,
                        i.Warehouse.WarehouseName,
                        i.OperatePersonID,
                        i.VerifyPersonID,
                        OperatePersonCode = i.OperatePerson.EmployeeCode,
                        OperatePersonName = i.OperatePerson.EmployeeName,
                        VerifyPersonCode = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeCode,
                        VerifyPersonName = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeName,
                        BillTypeCode = i.BillType.BillTypeCode,
                        BillTypeName = i.BillType.BillTypeName,
                        VerifyDate = i.VerifyDate == null ? string.Empty : ((DateTime)i.VerifyDate).ToString("yyyy-MM-dd HH:mm:ss"),
                        Status = WhatStatus(i.Status),
                        IsActive = i.IsActive == "1" ? "可用" : "不可用",
                        Description = i.Description,
                        UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                    });
            return new { total, rows = temp.ToArray() };
        }

        public bool Add(OutBillMaster outBillMaster, string userName, out string errorInfo)
        {
            errorInfo = string.Empty;
            var outbm = new OutBillMaster();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (employee != null)
            {
                try
                {
                    outbm.BillNo = outBillMaster.BillNo;
                    outbm.BillDate = outBillMaster.BillDate;
                    outbm.BillTypeCode = outBillMaster.BillTypeCode;
                    outbm.WarehouseCode = outBillMaster.WarehouseCode;
                    outbm.OperatePersonID = employee.ID;
                    outbm.Status = "1";
                    outbm.VerifyPersonID = outBillMaster.VerifyPersonID;
                    outbm.VerifyDate = outBillMaster.VerifyDate;
                    outbm.Description = outBillMaster.Description;
                    //outbm.IsActive = outBillMaster.IsActive;
                    outbm.IsActive = "1";
                    outbm.UpdateTime = DateTime.Now;
                    outbm.Origin = "1";

                    OutBillMasterRepository.Add(outbm);
                    OutBillMasterRepository.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    errorInfo = "添加失败！原因：" + e.Message;
                    return false;
                }
            }
            else
            {
                errorInfo = "找不到当前登陆用户！请重新登陆！";
                return false;
            }
        }

        public bool Delete(string BillNo, out string errorInfo)
        {
            errorInfo = string.Empty;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            if (ibm != null)
            {
                try
                {
                    //Del(OutBillDetailRepository, ibm.OutBillAllots);
                    Del(OutBillDetailRepository, ibm.OutBillDetails);
                    OutBillMasterRepository.Delete(ibm);
                    OutBillMasterRepository.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    errorInfo = "删除失败！原因：" + e.Message;
                    return false;
                }
            }
            else
            {
                errorInfo = "删除失败！未找到当前需要删除的数据！";
                return false;
            }
        }

        public bool Save(OutBillMaster outBillMaster, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == outBillMaster.BillNo && i.Status == "1");
            if (outbm != null)
            {
                try
                {
                    outbm.BillDate = outBillMaster.BillDate;
                    outbm.BillTypeCode = outBillMaster.BillTypeCode;
                    outbm.WarehouseCode = outBillMaster.WarehouseCode;
                    outbm.OperatePersonID = outBillMaster.OperatePersonID;
                    outbm.Status = "1";
                    outbm.VerifyPersonID = outBillMaster.VerifyPersonID;
                    outbm.VerifyDate = outBillMaster.VerifyDate;
                    outbm.Description = outBillMaster.Description;
                    outbm.IsActive = "1";
                    outbm.UpdateTime = DateTime.Now;
                    outbm.Origin = "1";

                    OutBillMasterRepository.SaveChanges();
                    result = true;
                }
                catch (Exception e)
                {
                    errorInfo = "删除失败！原因：" + e.Message;
                }
            }
            else
                errorInfo = "保存失败！没有找到这条数据！";
            return result;
        }

        /// <summary>
        /// 生成出库单号
        /// </summary>
        /// <param name="userName">登陆用户</param>
        /// <returns></returns>
        public object GenInBillNo(string userName)
        {
            string billno = "";
            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            var outBillMaster = outBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).ToArray().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (outBillMaster.Count() == 0)
            {
                billno = System.DateTime.Now.ToString("yyMMdd") + "0001" + "OU";
            }
            else
            {
                string billNoStr = outBillMaster.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                billno = System.DateTime.Now.ToString("yyMMdd") + newcode + "OU";
            }
            var findBillInfo = new
            {
                BillNo = billno,
                billNoDate = DateTime.Now.ToString("yyyy-MM-dd"),
                employeeID = employee == null ? string.Empty : employee.ID.ToString(),
                employeeCode = employee == null ? string.Empty : employee.EmployeeCode,
                employeeName = employee == null ? string.Empty : employee.EmployeeName

            };
            return findBillInfo;
        }

        /// <summary>
        /// 出库审核
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="userName">登陆用户</param>
        /// <returns></returns>
        public bool Audit(string billNo, string userName, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (outbm != null && outbm.Status == "1")
            {
                outbm.Status = "2";
                outbm.VerifyDate = DateTime.Now;
                outbm.UpdateTime = DateTime.Now;
                outbm.VerifyPersonID = employee.ID;
                OutBillMasterRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 出库反审
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns></returns>
        public bool AntiTrial(string billNo, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var outbm = OutBillMasterRepository.GetQueryable().Where(i => billNo.Contains(i.BillNo));
            if (outbm.Count() > 0)
            {
                foreach (var item in outbm.ToArray())
                {
                    if (item.Status == "2")
                    {
                        try
                        {
                            item.Status = "1";
                            item.VerifyDate = null;
                            item.UpdateTime = DateTime.Now;
                            item.VerifyPersonID = null;
                            OutBillMasterRepository.SaveChanges();
                            result = true;
                        }
                        catch (Exception e)
                        {
                            errorInfo = item.BillNo + "其他人员正在操作！无法保存！" + e.Message;
                        }
                    }
                    else
                        errorInfo = item.BillNo + "这条单据状态不是已审核！";
                }
            }
            else
                errorInfo = "保存失败！没有找到这些数据！";
            return result;
        }

        /// <summary>
        /// 出库结单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="errorInfo">错误信息</param>
        /// <returns></returns>
        public bool Settle(string billNo, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            if (outbm != null && outbm.Status == "5")
            {
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        //结单移库单,修改冻结量
                        var moveDetail = MoveBillDetailRepository.GetQueryable()
                                                                 .Where(m => m.BillNo == outbm.MoveBillMasterBillNo
                                                                     && m.Status != "2");
                        //结单出库单,修改冻结量
                        var outAllot = OutBillAllotRepository.GetQueryable()
                                                             .Where(o => o.BillNo == outbm.BillNo
                                                                 && o.Status != "2");

                        var sourceStorages = moveDetail.Select(m => m.OutStorage).ToArray();
                        var targetStorages = moveDetail.Select(m => m.InStorage).ToArray();
                        var storages = outAllot.Select(i => i.Storage).ToArray();

                        if (!Locker.Lock(storages)
                            || !Locker.Lock(sourceStorages)
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
                                    throw new Exception("储位的卷烟或入库冻结量与当前分配不符，信息可能被异常修改，不能结单！");
                                }
                            }
                        );
                        MoveBillDetailRepository.SaveChanges();

                        outAllot.AsParallel().ForAll(
                            (Action<OutBillAllot>)delegate(OutBillAllot o)
                            {
                                if (o.Storage.ProductCode == o.ProductCode
                                    && o.Storage.OutFrozenQuantity >= o.AllotQuantity)
                                {
                                    o.Storage.OutFrozenQuantity -= o.AllotQuantity;
                                    o.Storage.LockTag = string.Empty;
                                }
                                else
                                {
                                    throw new Exception("储位的卷烟或入库冻结量与当前分配不符，信息可能被异常修改，不能结单！");
                                }
                            }
                        );

                        if (outbm.MoveBillMaster != null)
                        {
                            outbm.MoveBillMaster.Status = "4";
                            outbm.MoveBillMaster.UpdateTime = DateTime.Now;
                        }

                        outbm.Status = "6";
                        outbm.UpdateTime = DateTime.Now;
                        OutBillMasterRepository.SaveChanges();
                        scope.Complete();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        errorInfo = "出库单结单出错！原因：" + e.Message;
                        return false;
                    }
                }
            }
            return result;
        }
    }
}
