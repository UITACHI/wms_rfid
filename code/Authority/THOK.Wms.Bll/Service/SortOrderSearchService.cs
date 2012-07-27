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
    public class SortOrderSearchService : ServiceBase<SortOrder>, ISortOrderSearchService
    {
        [Dependency]
        public ISortOrderSearchRepository SortOrderSearchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortOrderSearch 成员

        public object GetDetails(int page, int rows, string OrderID, string OrderDate)
        {
            IQueryable<SortOrder> SortOrderQuery = SortOrderSearchRepository.GetQueryable();
            var SortOrderSearch = SortOrderQuery.Where(i => i.OrderID.Contains(OrderID)).OrderBy(i => i.OrderID).AsEnumerable().Select(i => new
            {
                i.OrderID,
                i.OrderDate,
                i.OrderType,
                i.CustomerCode,
                i.CustomerName,
                i.QuantitySum,
                i.DetailNum
            });
            int total = SortOrderSearch.Count();
            SortOrderSearch = SortOrderSearch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = SortOrderSearch.ToArray() };
        }

        #endregion

    }
}
