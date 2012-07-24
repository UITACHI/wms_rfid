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
                var inBillDetail = inBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { 
                    i.ID, 
                    i.BillNo, 
                    i.ProductCode, 
                    i.Product.ProductName, 
                    
                    i.UnitCode, 
                    i.Unit.UnitName, 
                    i.BillQuantity, 
                    i.RealQuantity, 
                    i.Price, 
                    i.Description });
                int total = inBillDetail.Count();
                inBillDetail = inBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = inBillDetail.ToArray() };
            }
            return "";
        }
        #endregion
    }
}
