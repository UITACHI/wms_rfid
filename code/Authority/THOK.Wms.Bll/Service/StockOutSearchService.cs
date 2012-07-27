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
    public class StockOutSearchService : ServiceBase<OutBillMaster>, IStockOutSearchService
    {
        [Dependency]
        public IStockOutSearchRepository StockOutSearchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IStockOutSearch 成员

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

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status)
        {
            IQueryable<OutBillMaster> StockOutQuery = StockOutSearchRepository.GetQueryable();
            var StockOutSearch = StockOutQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
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
                UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
            });
            int total = StockOutSearch.Count();
            StockOutSearch = StockOutSearch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = StockOutSearch.ToArray() };
        }

        #endregion

    }
}
