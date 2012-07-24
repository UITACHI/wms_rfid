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
    public class StockIntoSearchService :ServiceBase<InBillMaster>,IStockIntoSearchService
    {
        [Dependency]
        public IStockIntoSearchRepository StockIntoSearchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IStockIntoSearch 成员

        public object GetDetails(int page, int rows, string BillNo, string BillDate,string OperatePersonCode, string Status)
        {
            IQueryable<InBillMaster> StockIntoQuery = StockIntoSearchRepository.GetQueryable();
            var StockIntoSearch = StockIntoQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { 
                i.BillNo, 
                i.Warehouse.WarehouseName,
                BillDate = i.BillDate.ToString("yyyy-MM-dd hh:mm:ss"),
                OperatePersonName = i.OperatePerson.EmployeeName, 
                i.OperatePersonID,
                Status = i.Status == "1" ? "可用" : "不可用",
                VerifyPersonName = i.VerifyPerson.EmployeeName,
                i.VerifyDate,
                Description = i.Description, 
                UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            int total = StockIntoSearch.Count();
            StockIntoSearch = StockIntoSearch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = StockIntoSearch.ToArray() };
        }

        #endregion

    }
}
