using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class OrderSearchDetailService : ServiceBase<SortOrderDetail>, IOrderSearchDetailService
    {
        [Dependency]
        public IOrderSearchDetailRepository OrderSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IOrderSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string OrderID)
        {
            if (OrderID != "" && OrderID != null)
            {
                IQueryable<SortOrderDetail> OrderOrderDetailQuery = OrderSearchDetailRepository.GetQueryable();
                var OrderOrderDetail = OrderOrderDetailQuery.Where(i => i.OrderID.Contains(OrderID)).OrderBy(i => i.OrderID).AsEnumerable().Select(i => new
                {
                    i.OrderID,
                    i.Price,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.OrderDetailID,
                    i.RealQuantity,
                    i.UnitCode,
                    i.UnitName,
                    i.Amount
                });
                int total = OrderOrderDetail.Count();
                OrderOrderDetail = OrderOrderDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = OrderOrderDetail.ToArray() };
            }
            return "";
        }
        #endregion
    }
}
