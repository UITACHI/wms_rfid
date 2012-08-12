using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class StockCheckSearchService : ServiceBase<CheckBillMaster>, IStockCheckSearchService
    {
        [Dependency]
        public IStockCheckSearchRepository StockCheckSearchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IStockCheckSearch 成员

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
                    statusStr = "执行中";
                    break;
                case "4":
                    statusStr = "已入库";
                    break;
                case "5":
                    statusStr = "已生成损益";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string WarehouseCode, string BeginDate, string EndDate, string OperatePersonCode, string CheckPersonCode, string Operate_Status)
        {
            IQueryable<CheckBillMaster> StockCheckQuery = StockCheckSearchRepository.GetQueryable();
            var StockCheckSearch = StockCheckQuery.Where(i => i.BillNo.Contains(BillNo)
                                                         && i.WarehouseCode.Contains(WarehouseCode)
                                                         && i.OperatePerson.EmployeeCode.Contains(OperatePersonCode)
                                                         //&& i.VerifyPerson.EmployeeCode.Contains(CheckPersonCode)
                                                         && i.Status.Contains(Operate_Status))
                                                .OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                 { 
                i.BillNo, 
                i.Warehouse.WarehouseName,
                BillDate = i.BillDate.ToString("yyyy-MM-dd hh:mm:ss"),
                OperatePersonName = i.OperatePerson.EmployeeName,
                i.OperatePersonID,
                Status = WhatStatus(i.Status),
                VerifyPersonName = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeName,
                VerifyDate = (i.VerifyDate == null ? string.Empty : ((DateTime)i.VerifyDate).ToString("yyyy-MM-dd hh:mm:ss")),
                Description = i.Description, 
                UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });

            if (!BeginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(BeginDate);
                StockCheckSearch = StockCheckSearch.Where(i => Convert.ToDateTime(i.BillDate) >= begin);
            }

            if (!EndDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(EndDate);
                StockCheckSearch = StockCheckSearch.Where(i => Convert.ToDateTime(i.BillDate) <= end);
            }

            int total = StockCheckSearch.Count();
            StockCheckSearch = StockCheckSearch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = StockCheckSearch.ToArray() };
        }

        #endregion

    }
}
