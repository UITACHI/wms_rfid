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
    public class CheckSearchDetailService : ServiceBase<CheckBillDetail>, ICheckSearchDetailService
    {
        [Dependency]
        public ICheckSearchDetailRepository CheckSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICheckSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<CheckBillDetail> checkBillDetailQuery = CheckSearchDetailRepository.GetQueryable();
                var checkBillDetail = checkBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.UnitCode,
                    i.Unit.UnitName,
                    i.CellCode,
                    i.Cell.CellName,
                    i.StorageCode,
                    i.RealQuantity,
                    i.Quantity,
                    DifiierQuantity = i.RealQuantity-i.Quantity
                });
                int total = checkBillDetail.Count();
                checkBillDetail = checkBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = checkBillDetail.ToArray() };
            }
            return "";
        }
        #endregion
    }
}
