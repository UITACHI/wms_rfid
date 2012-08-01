using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;

namespace THOK.Wms.Bll.Service
{
    public class ProfitLossBillMasterService:ServiceBase<ProfitLossBillMaster>,IProfitLossBillMasterService
    {
        [Dependency]
        public IProfitLossBillMasterRepository ProfitLossBillMasterRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

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

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<ProfitLossBillMaster> ProfitLossBillMasterQuery = ProfitLossBillMasterRepository.GetQueryable();
            var ProfitLossBillMaster = ProfitLossBillMasterQuery.Where(i => i.BillNo.Contains(BillNo)
                && i.Status != "3").OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
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
            if (!IsActive.Equals(""))
            {
                ProfitLossBillMaster = ProfitLossBillMaster.Where(i =>
                    i.BillNo.Contains(BillNo)
                    && i.IsActive.Contains(IsActive)
                    && i.Status != "3").OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                    {
                        i.BillNo,
                        i.BillDate,
                        i.OperatePersonID,
                        i.WarehouseCode,
                        i.BillTypeCode,
                        i.BillTypeName,
                        i.CheckBillNo,
                        i.WarehouseName,
                        i.OperatePersonCode,
                        i.OperatePersonName,
                        i.VerifyPersonID,
                        i.VerifyPersonName,
                        i.VerifyDate,
                        Status = WhatStatus(i.Status),
                        IsActive = i.IsActive == "1" ? "可用" : "不可用",
                        Description = i.Description,
                        UpdateTime = i.UpdateTime
                    });
            }
            int total = ProfitLossBillMaster.Count();
            ProfitLossBillMaster = ProfitLossBillMaster.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = ProfitLossBillMaster.ToArray() };
        }

        public bool Add(ProfitLossBillMaster profitLossBillMaster, string userName)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(ProfitLossBillMaster profitLossBillMaster)
        {
            throw new NotImplementedException();
        }

        public object GenProfitLossBillNo(string userName)
        {
            throw new NotImplementedException();
        }

        public bool Audit(string BillNo, string userName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
