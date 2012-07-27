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
    public class SortOrderService : ServiceBase<SortOrder>, ISortOrderService
    {
        [Dependency]
        public ISortOrderRepository SortOrderRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortOrderService 成员

        public object GetDetails(int page, int rows, string OrderID, string orderDate, string CustomerCode, string CustomerName)
        {
            IQueryable<SortOrder> sortOrderQuery = SortOrderRepository.GetQueryable();
            var sortOrder = sortOrderQuery.Where(s => s.OrderID.Contains(OrderID) && s.CustomerCode.Contains(CustomerCode) && s.CustomerName.Contains(CustomerName));
            if (!orderDate.Equals(string.Empty) || orderDate!=null)
            {
                sortOrder = sortOrder.Where(i => i.OrderDate == orderDate);
            }
            var temp = sortOrder.AsEnumerable().OrderBy(t => t.OrderID).Select(s => new
            {
                s.OrderID,
                s.CompanyCode,
                s.SaleRegionCode,
                s.OrderDate,
                s.OrderType,
                s.CustomerCode,
                s.CustomerName,
                s.QuantitySum,
                s.AmountSum,
                s.DetailNum,
                s.DeliverOrder,
                s.DeliverDate,
                IsActive = s.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                s.Description
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        #endregion
    }
}
