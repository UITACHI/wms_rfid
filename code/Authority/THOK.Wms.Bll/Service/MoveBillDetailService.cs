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
    public class MoveBillDetailService:ServiceBase<MoveBillDetail>, IMoveBillDetailService
    {
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IMoveBillDetail 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<MoveBillDetail> MoveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
                var moveBillDetail = MoveBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.OutCellCode,
                    OutCellName=i.OutCell.CellName,
                    i.OutStorageCode,
                    i.InCellCode,
                    InCellName=i.InCell.CellName,
                    i.InStorageCode,
                    i.UnitCode,
                    i.Unit.UnitName,
                    i.RealQuantity,
                    i.OperatePersonID,
                    i.OperatePerson.EmployeeName,
                    StartTime=((DateTime)i.StartTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    FinishTime=((DateTime)i.FinishTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    i.Status
                });
                int total = moveBillDetail.Count();
                moveBillDetail = moveBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = moveBillDetail.ToArray() };
            }
            return "";
        }

        public new bool Add(MoveBillDetail moveBillDetail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Save(MoveBillDetail moveBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
