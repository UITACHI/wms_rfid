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
    public class IntoSearchDetailService : ServiceBase<InBillAllot>,IIntoSearchDetailService
    {
        [Dependency]
        public IIntoSearchDetailRepository IntoSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IIntoSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<InBillAllot> inBillDetailQuery = IntoSearchDetailRepository.GetQueryable();
                var inBillAllot = inBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                { 
                    i.ID, 
                    i.BillNo, 
                    i.ProductCode, 
                    i.Product.ProductName,
                    i.AllotQuantity,
                    i.CellCode,
                    i.Cell.CellName,
                    i.StorageCode,
                    i.UnitCode,
                    i.Unit.UnitName,
                    i.RealQuantity,
                    i.Status 
                });
                int total = inBillAllot.Count();
                inBillAllot = inBillAllot.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = inBillAllot.ToArray() };
            }
            return "";
        }
        #endregion
    }
}
