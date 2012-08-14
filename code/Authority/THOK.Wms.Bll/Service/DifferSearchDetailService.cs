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
    public class DifferSearchDetailService : ServiceBase<ProfitLossBillDetail>, IDifferSearchDetailService
    {
        [Dependency]
        public IDifferSearchDetailRepository DifferSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IDifferSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<ProfitLossBillDetail> DifferBillDetailQuery = DifferSearchDetailRepository.GetQueryable();
                var DifferBillDetail = DifferBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.UnitCode,
                    i.Unit.UnitName,
                    i.Product.ProductName,
                    i.CellCode,
                    i.Storage.Cell.CellName,
                    i.Quantity
                });
                int total = DifferBillDetail.Count();
                DifferBillDetail = DifferBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = DifferBillDetail.ToArray() };
            }
            return "";
        }
        #endregion
    }
}
