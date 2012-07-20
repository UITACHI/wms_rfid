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
    public class OutBillDetailService:ServiceBase<OutBillDetail>,IOutBillDetailService
    {
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IOutBillDetailService 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
                var outBillDetail = outBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new {i.ID, i.BillNo, i.ProductCode, i.Product.ProductName, i.UnitCode, i.Unit.UnitName, i.BillQuantity, i.RealQuantity, i.Price, i.Description });
                int total = outBillDetail.Count();
                outBillDetail = outBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = outBillDetail.ToArray() };
            }
            return "";
        }

        public bool Add(OutBillDetail outBillDetail)
        {
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            var isExistProduct = outBillDetailQuery.FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ProductCode == outBillDetail.ProductCode);
            if (isExistProduct == null)
            {
                var ibd = new OutBillDetail();
                ibd.BillNo = outBillDetail.BillNo;
                ibd.ProductCode = outBillDetail.ProductCode;
                ibd.UnitCode = outBillDetail.UnitCode;
                ibd.Price = outBillDetail.Price;
                ibd.BillQuantity = outBillDetail.BillQuantity;
                ibd.AllotQuantity = 0;
                ibd.RealQuantity = 0;
                ibd.Description = outBillDetail.Description;

                OutBillDetailRepository.Add(ibd);
                OutBillDetailRepository.SaveChanges();
            }
            else
            {
                var ibd = outBillDetailQuery.FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ProductCode == outBillDetail.ProductCode);
                ibd.BillQuantity = ibd.BillQuantity + outBillDetail.BillQuantity;
                OutBillDetailRepository.SaveChanges();
            }
            return true;
        }

        public bool Delete(string BillNo, string ID)
        {
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            int id = Convert.ToInt32(ID);
            var outBillDetail = outBillDetailQuery.FirstOrDefault(o => o.BillNo == BillNo && o.ID == id);
            if (outBillDetail != null)
            {
                OutBillDetailRepository.Delete(outBillDetail);
                OutBillDetailRepository.SaveChanges();
            }
            return true;
        }

        public bool Save(OutBillDetail inBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
