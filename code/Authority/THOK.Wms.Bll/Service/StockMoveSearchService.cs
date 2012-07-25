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

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status)
        {
            IQueryable<MoveBillMaster> StockMoveQuery = StockMoveSearchRepository.GetQueryable();
            var StockMoveSearch = StockMoveQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
            {
                i.BillNo,
                i.Warehouse.WarehouseName,
                BillDate = i.BillDate.ToString("yyyy-MM-dd hh:mm:ss"),
                OperatePersonName = i.OperatePerson.EmployeeName,
                i.OperatePersonID,
                Status = i.Status == "1" ? "可用" : "不可用",
                VerifyPersonName = i.VerifyPerson.EmployeeName,
                i.VerifyDate,
                Description = i.Description,
                UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
            });
            int total = StockMoveSearch.Count();
            StockMoveSearch = StockMoveSearch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = StockMoveSearch.ToArray() };
        }

        #endregion

    }
}
