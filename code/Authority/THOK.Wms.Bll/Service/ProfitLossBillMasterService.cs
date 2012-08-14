using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;
using THOK.Wms.SignalR;
using THOK.Wms.SignalR.Common;

namespace THOK.Wms.Bll.Service
{
    public class ProfitLossBillMasterService:ServiceBase<ProfitLossBillMaster>,IProfitLossBillMasterService
    {
        [Dependency]
        public IProfitLossBillMasterRepository ProfitLossBillMasterRepository { get; set; }
        [Dependency]
        public IProfitLossBillDetailRepository ProfitLossBillDetailRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string resultStr = "";//错误信息字符串

        /// <summary>
        /// 判断处理状态
        /// </summary>
        /// <param name="status">数据库查询出来的状态值</param>
        /// <returns></returns>
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
                    statusStr = "已更新库存";
                    break;
            }
            return statusStr;
        }

        #region IProfitLossBillMasterService 成员

        /// <summary>
        /// 查询损益单主单明细
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="BillNo">损益单号</param>
        /// <param name="BillDate">损益单日期</param>
        /// <param name="OperatePersonCode">操作员</param>
        /// <param name="Status">状态</param>
        /// <param name="IsActive">是否可用</param>
        /// <returns></returns>
        public object GetDetails(int page, int rows, string BillNo, string WareHouseCode, string BeginDate, string EndDate, string OperatePersonCode, string CheckPersonCode, string Status, string IsActive)
        {
            IQueryable<ProfitLossBillMaster> ProfitLossBillMasterQuery = ProfitLossBillMasterRepository.GetQueryable();
            var ProfitLossBillMaster = ProfitLossBillMasterQuery.Where(i => i.BillNo.Contains(BillNo)
                    && i.Status != "2"
                    && i.WarehouseCode.Contains(WareHouseCode)
                    && i.OperatePerson.EmployeeCode.Contains(OperatePersonCode)
                    //|| i.VerifyPerson.EmployeeCode.Contains(CheckPersonCode)
                    && i.Status.Contains(Status))
                    .OrderByDescending(t => t.BillDate)
                    .OrderByDescending(t => t.BillNo)
                    .Select(p => p);
            if (!BeginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(BeginDate);
                ProfitLossBillMaster = ProfitLossBillMaster.Where(i => i.BillDate >= begin);
            }

            if (!EndDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(EndDate).AddDays(1);
                ProfitLossBillMaster = ProfitLossBillMaster.Where(i => i.BillDate <= end);
            }

            int total = ProfitLossBillMaster.Count();
            ProfitLossBillMaster = ProfitLossBillMaster.Skip((page - 1) * rows).Take(rows);

            var temp=ProfitLossBillMaster.ToArray().AsEnumerable().Select(i=>new
            {
                    i.BillNo,
                    BillDate = i.BillDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    i.OperatePersonID,
                    i.WarehouseCode,
                    i.BillTypeCode,
                    i.BillType.BillTypeName,
                    i.CheckBillNo,
                    i.Warehouse.WarehouseName,
                    OperatePersonCode = i.OperatePerson.EmployeeCode,
                    OperatePersonName = i.OperatePerson.EmployeeName,
                    VerifyPersonID = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeCode,
                    VerifyPersonName = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeName,
                    VerifyDate = (i.VerifyDate == null ? "" : ((DateTime)i.VerifyDate).ToString("yyyy-MM-dd HH:mm:ss")),
                    Status = WhatStatus(i.Status),
                    IsActive = i.IsActive == "1" ? "可用" : "不可用",
                    Description = i.Description,
                    UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            return new { total, rows = temp.ToArray() };
        }

        /// <summary>
        /// 新增损益单主单
        /// </summary>
        /// <param name="profitLossBillMaster">损益单主单</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public bool Add(ProfitLossBillMaster profitLossBillMaster, string userName)
        {
            bool result = false;
            var pbm = new ProfitLossBillMaster();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (employee != null)
            {
                pbm.BillNo = profitLossBillMaster.BillNo;
                pbm.BillDate = profitLossBillMaster.BillDate;
                pbm.BillTypeCode = profitLossBillMaster.BillTypeCode;
                pbm.WarehouseCode = profitLossBillMaster.WarehouseCode;
                pbm.OperatePersonID = employee.ID;
                pbm.Status = "1";
                pbm.VerifyPersonID = profitLossBillMaster.VerifyPersonID;
                pbm.VerifyDate = profitLossBillMaster.VerifyDate;
                pbm.Description = profitLossBillMaster.Description;
                //pbm.IsActive = profitLossBillMaster.IsActive;
                pbm.IsActive = "1";
                pbm.UpdateTime = DateTime.Now;

                ProfitLossBillMasterRepository.Add(pbm);
                ProfitLossBillMasterRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 删除损益单主单
        /// </summary>
        /// <param name="BillNo">损益单号</param>
        /// <returns></returns>
        public bool Delete(string BillNo,out string strResult)
        {
            bool result = false;
            var pbm = ProfitLossBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            if (LockBillMaster(BillNo))
            {
                if (pbm != null)
                {
                    DeleteProfitLossBillDetail(pbm);
                    ProfitLossBillMasterRepository.Delete(pbm);
                    ProfitLossBillMasterRepository.SaveChanges();
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            strResult = resultStr;
            return result;
        }

        /// <summary>
        /// 批量删除损益细单
        /// </summary>
        /// <param name="profitLossBillMaster">损益主单</param>
        public void DeleteProfitLossBillDetail(ProfitLossBillMaster profitLossBillMaster)
        {
            if (profitLossBillMaster != null)
            {
                foreach (var detail in profitLossBillMaster.ProfitLossBillDetails)
                {
                    var Storage = Locker.LockStorage(detail.Storage, detail.Product);
                    if (Storage != null)
                    {
                        if (detail.Quantity > 0)
                        {
                            Storage.InFrozenQuantity -= detail.Quantity;
                        }
                        else
                        {
                            Storage.OutFrozenQuantity -= Math.Abs(detail.Quantity);
                        }
                        Storage.LockTag = string.Empty;
                        detail.Quantity = 0;
                    }
                }
                var details = profitLossBillMaster.ProfitLossBillDetails.Where(d => d.Quantity == 0)
                                                            .Select(d => d);
                ProfitLossBillDetailRepository.Delete(details.ToArray());
                ProfitLossBillDetailRepository.SaveChanges();
            }
        }

        /// <summary>
        /// 保存损益单主单
        /// </summary>
        /// <param name="profitLossBillMaster">损益单主单</param>
        /// <returns></returns>
        public bool Save(ProfitLossBillMaster profitLossBillMaster, out string strResult)
        {
            bool result = false;
            var pbm = ProfitLossBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == profitLossBillMaster.BillNo && i.Status == "1");
            if (LockBillMaster(profitLossBillMaster.BillNo))
            {
                if (pbm != null)
                {
                    pbm.BillDate = profitLossBillMaster.BillDate;
                    pbm.BillTypeCode = profitLossBillMaster.BillTypeCode;
                    pbm.WarehouseCode = profitLossBillMaster.WarehouseCode;
                    pbm.OperatePersonID = profitLossBillMaster.OperatePersonID;
                    pbm.Status = "1";
                    pbm.VerifyPersonID = profitLossBillMaster.VerifyPersonID;
                    pbm.VerifyDate = profitLossBillMaster.VerifyDate;
                    pbm.Description = profitLossBillMaster.Description;
                    //pbm.IsActive = profitLossBillMaster.IsActive;
                    pbm.IsActive = "1";
                    pbm.UpdateTime = DateTime.Now;

                    pbm.LockTag = string.Empty;
                    ProfitLossBillMasterRepository.SaveChanges();
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            strResult = resultStr;
            return result;
        }

         /// <summary>
         /// 生成损益主单单号
         /// </summary>
         /// <param name="userName">用户名</param>
         /// <returns></returns>
        public object GenProfitLossBillNo(string userName)
        {
            IQueryable<ProfitLossBillMaster> profitLossBillMasterQuery = ProfitLossBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            string billNo = "";
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            var profitLossBillMaster = profitLossBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).ToArray().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (profitLossBillMaster.Count() == 0)
            {
                billNo = System.DateTime.Now.ToString("yyMMdd") + "0001" + "PL";
            }
            else
            {
                string billNoStr = profitLossBillMaster.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                billNo = System.DateTime.Now.ToString("yyMMdd") + newcode + "PL";
            }
            var findBillInfo = new
            {
                BillNo = billNo,
                billNoDate = DateTime.Now.ToString("yyyy-MM-dd"),
                employeeID = employee == null ? "" : employee.ID.ToString(),
                employeeCode = employee == null ? "" : employee.EmployeeCode.ToString(),
                employeeName = employee == null ? "" : employee.EmployeeName.ToString()
            };
            return findBillInfo;
        }

        /// <summary>
        /// 损益主单审核
        /// </summary>
        /// <param name="BillNo">损益单号</param>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public bool Audit(string BillNo, string userName, out string strResult)
        {
            bool result = false;
            var pbm = ProfitLossBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (LockBillMaster(BillNo))
            {
                if (pbm != null)
                {
                    ChangeStorages(pbm);
                    pbm.Status = "2";
                    pbm.VerifyDate = DateTime.Now;
                    pbm.UpdateTime = DateTime.Now;
                    pbm.VerifyPersonID = employee.ID;

                    pbm.LockTag = string.Empty;
                    ProfitLossBillMasterRepository.SaveChanges();
                    result = true;
                }
            }
            else
            {
                result = false;
            }
            strResult = resultStr;
            return result;
        }

        /// <summary>
        /// 批量修改库存信息
        /// </summary>
        /// <param name="profitLossBillMaster">损益主单</param>
        public void ChangeStorages(ProfitLossBillMaster profitLossBillMaster)
        {
            if (profitLossBillMaster!=null)
            {
                foreach(var detail in profitLossBillMaster.ProfitLossBillDetails)
                {
                    var Storage = Locker.LockStorage(detail.Storage, detail.Product);
                    if (detail.Quantity > 0)
                    {
                        Storage.InFrozenQuantity -= detail.Quantity;
                        Storage.Quantity += detail.Quantity;
                    }
                    else
                    {
                        Storage.OutFrozenQuantity -= detail.Quantity;
                        Storage.Quantity -=Math.Abs(detail.Quantity);
                    }
                    Storage.LockTag = string.Empty;
                }
            }
        }

        #endregion

        #region IProfitLossBillMasterService 成员


        /// <summary>
        /// 对损益主单进行加锁
        /// </summary>
        /// <param name="BillNo">损益单号</param>
        /// <param name="strResult">提示信息文本</param>
        /// <returns></returns>
        public bool LockBillMaster(string BillNo)
        {
            bool result = false;
            var pbm = ProfitLossBillMasterRepository.GetQueryable().FirstOrDefault(p => p.BillNo == BillNo && p.Status == "1");
            if (pbm != null)
            {
                if (string.IsNullOrEmpty(pbm.LockTag))
                {
                    pbm.LockTag = BillNo;
                    ProfitLossBillMasterRepository.SaveChanges();
                    result = true;
                }
                else
                {
                    resultStr = "当前订单其他人正在操作，请稍候重试！";
                    result = false;
                }
            }
            else
            {
                resultStr = "当前单据的状态不是已录入状态或者该单据已被删除无法编辑，请刷新页面！";
                result = false;
            }
            return result;
        }

        #endregion
    }
}
