using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class OutBillMasterService:ServiceBase<OutBillMaster>,IOutBillMasterService
    {
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }

        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }

        
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IOutBillMasterService 成员

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
                    statusStr = "已入库";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<OutBillMaster> OutBillMasterQuery = OutBillMasterRepository.GetQueryable();
            var outBillMaster = OutBillMasterQuery.Where(i => i.BillNo.Contains(BillNo) && i.OperatePerson.EmployeeName.Contains(OperatePersonCode));
            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                outBillMaster = outBillMaster.Where(i => i.BillDate >= begin);
            }

            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate).AddDays(+1);
                outBillMaster = outBillMaster.Where(i => i.BillDate <= end);
            }

            if (!Status.Equals(string.Empty))
            {
                outBillMaster = outBillMaster.Where(i => i.Status.Contains(Status) && i.Status != "6");
            }

            if (!IsActive.Equals(string.Empty))
            {
                outBillMaster = outBillMaster.Where(i => i.IsActive.Contains(IsActive));
            }

            var temp = outBillMaster.AsEnumerable()
                                    .OrderByDescending(t => t.BillDate)
                                    .OrderByDescending(t => t.BillNo)
                                    .Select(i => new
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

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public bool Add(OutBillMaster outBillMaster, string userName)
        {
            var outbm = new OutBillMaster();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (employee != null)
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
                outbm.IsActive = outBillMaster.IsActive;
                outbm.UpdateTime = DateTime.Now;

                OutBillMasterRepository.Add(outbm);
                OutBillMasterRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(string BillNo)
        {
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            if (ibm != null)
            {
                //Del(OutBillDetailRepository, ibm.OutBillAllots);
                Del(OutBillDetailRepository, ibm.OutBillDetails);
                OutBillMasterRepository.Delete(ibm);
                OutBillMasterRepository.SaveChanges();
            }
            return true;
        }

        public bool Save(OutBillMaster outBillMaster)
        {
            bool result = false;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == outBillMaster.BillNo && i.Status == "1");
            if (outbm != null)
            {
                outbm.BillDate = outBillMaster.BillDate;
                outbm.BillTypeCode = outBillMaster.BillTypeCode;
                outbm.WarehouseCode = outBillMaster.WarehouseCode;
                outbm.OperatePersonID = outBillMaster.OperatePersonID;
                outbm.Status = "1";
                outbm.VerifyPersonID = outBillMaster.VerifyPersonID;
                outbm.VerifyDate = outBillMaster.VerifyDate;
                outbm.Description = outBillMaster.Description;
                outbm.IsActive = outBillMaster.IsActive;
                outbm.UpdateTime = DateTime.Now;

                OutBillMasterRepository.SaveChanges();
                result = true;
            }
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
            var outBillMaster = outBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).AsEnumerable().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (outBillMaster.Count() == 0)
            {
                billno = System.DateTime.Now.ToString("yyMMdd") + "0001" + "CK";
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
                billno = System.DateTime.Now.ToString("yyMMdd") + newcode + "CK";
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
        public bool Audit(string billNo, string userName)
        {
            bool result = false;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (outbm != null && outbm.Status=="1")
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
        public bool AntiTrial(string billNo)
        {
            bool result = false;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            if (outbm != null && outbm.Status == "2")
            {
                outbm.Status = "1";
                outbm.VerifyDate = null;
                outbm.UpdateTime = DateTime.Now;
                outbm.VerifyPersonID = null;
                OutBillMasterRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        #endregion
    }
}
