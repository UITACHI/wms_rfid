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
    public class MoveSearchDetailService : ServiceBase<MoveBillDetail>, IMoveSearchDetailService
    {
        [Dependency]
        public IMoveSearchDetailRepository MoveSearchDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IMoveSearchDetailRepository 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<MoveBillDetail> MoveBillDetailQuery = MoveSearchDetailRepository.GetQueryable();
                var MoveBillDetail = MoveBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.UnitCode,
                    i.Unit.UnitName,  
                    i.InCellCode,
                    PlaceName_In=i.InCell.CellName,
                    i.OutCellCode,
                    PlaceName_Out=i.OutCell.CellName,
                    i.RealQuantity
                });
                int total = MoveBillDetail.Count();
                MoveBillDetail = MoveBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = MoveBillDetail.ToArray() };
            }
            return "";
        }
        #endregion
    }
}
