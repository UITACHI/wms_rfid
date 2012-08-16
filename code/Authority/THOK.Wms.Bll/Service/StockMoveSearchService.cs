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
    public class StockMoveSearchService : ServiceBase<MoveBillMaster>, IStockMoveSearchService
    {
        [Dependency]
        public IStockMoveSearchRepository StockMoveSearchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IStockMoveSearch 成员

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
                    statusStr = "已结单";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string WarehouseCode, string BeginDate, string EndDate, string OperatePersonCode, string CheckPersonCode, string Operate_Status)
        {
            IQueryable<MoveBillMaster> StockMoveQuery = StockMoveSearchRepository.GetQueryable();
            var StockMoveSearch = StockMoveQuery.Where(i => i.BillNo.Contains(BillNo)
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
                                                    UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
                                                });

            if (!BeginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(BeginDate);
                StockMoveSearch = StockMoveSearch.Where(i => Convert.ToDateTime(i.BillDate) >= begin);
            }

            if (!EndDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(EndDate);
                StockMoveSearch = StockMoveSearch.Where(i => Convert.ToDateTime(i.BillDate) <= end);
            }

            int total = StockMoveSearch.Count();
            StockMoveSearch = StockMoveSearch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = StockMoveSearch.ToArray() };
        }

        #endregion

    }
}
