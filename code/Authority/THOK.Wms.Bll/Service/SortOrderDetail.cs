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
    public class SortOrderDetailService : ServiceBase<SortOrderDetail>, ISortOrderDetailService
    {
        [Dependency]
        public ISortOrderDetailRepository SortOrderDetailRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortOrderDetailService 成员

        public object GetDetails(int page, int rows, string OrderID)
        {
            if (OrderID != string.Empty && OrderID != null)
            {
                IQueryable<SortOrderDetail> sortOrderDetailQuery = SortOrderDetailRepository.GetQueryable();
                var outBillDetail = sortOrderDetailQuery.Where(i => i.OrderID.Contains(OrderID)).OrderBy(i => i.OrderID).AsEnumerable().Select(i => new
                {
                    i.OrderDetailID,
                    i.OrderID,
                    i.Product.ProductCode,
                    i.Product.ProductName,
                    i.UnitCode,
                    i.UnitName,
                    i.DemandQuantity,
                    i.RealQuantity,
                    i.Price,
                    i.Amount,
                    i.UnitQuantity
                });
                int total = outBillDetail.Count();
                outBillDetail = outBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = outBillDetail.ToArray() };
            }
            return "";
        }

        #endregion
    }
}
