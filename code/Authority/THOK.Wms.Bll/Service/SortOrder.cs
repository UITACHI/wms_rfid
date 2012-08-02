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

        public object GetDetails(int page, int rows, string OrderID, string orderDate)
        {
            if (orderDate == string.Empty || orderDate == null)
            {
                orderDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                orderDate = Convert.ToDateTime(orderDate).ToString("yyyyMMdd");
            }
            IQueryable<SortOrder> sortOrderQuery = SortOrderRepository.GetQueryable();
            var sortOrder = sortOrderQuery.Where(s => s.OrderDate.Contains(orderDate));
            if (OrderID != string.Empty && OrderID != null)
            {
                sortOrder = sortOrder.Where(s => s.OrderID.Contains(OrderID));
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
                s.DeliverLineCode,
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


        public object GetDetails(string orderDate)
        {
            if (orderDate == string.Empty || orderDate == null)
            {
                orderDate = DateTime.Now.ToString("yyyyMMdd");
            }
            else
            {
                orderDate = Convert.ToDateTime(orderDate).ToString("yyyyMMdd");
            }
            IQueryable<SortOrder> sortOrderQuery = SortOrderRepository.GetQueryable();
            var sortOrder = sortOrderQuery.Where(s => s.OrderDate == orderDate).GroupBy(s => s.DeliverLine)
                                          .Select(s => new
                                          {
                                              DeliverLineCode = s.Key.DeliverLineCode,
                                              DeliverLineName = s.Key.DeliverLineName,
                                              OrderDate = orderDate,
                                              Quantity = s.Sum(p => p.QuantitySum),
                                              AmountSum = s.Sum(p => p.AmountSum),
                                              DetailNum = s.Sum(p => p.DetailNum),
                                              IsActive="1"
                                          });

            return sortOrder.ToArray();
        }

        #endregion
    }
}
