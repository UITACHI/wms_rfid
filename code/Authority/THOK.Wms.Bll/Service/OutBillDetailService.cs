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
        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

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
                var outBillDetail = outBillDetailQuery.Where(i => i.BillNo.Contains(BillNo))
                                                      .OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                                                      {
                                                          i.ID,
                                                          i.BillNo,
                                                          i.ProductCode,
                                                          i.Product.ProductName,
                                                          i.UnitCode,
                                                          i.Unit.UnitName,
                                                          BillQuantity = i.BillQuantity / i.Unit.Count,
                                                          i.RealQuantity,
                                                          i.Price,
                                                          i.Description
                                                      });
                int total = outBillDetail.Count();
                outBillDetail = outBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = outBillDetail.ToArray() };
            }
            return "";
        }

        public bool Add(OutBillDetail outBillDetail)
        {
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            var isExistProduct = outBillDetailQuery.FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ProductCode == outBillDetail.ProductCode&&i.UnitCode==outBillDetail.UnitCode);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == outBillDetail.UnitCode);
            if (isExistProduct == null)
            {
                var ibd = new OutBillDetail();
                ibd.BillNo = outBillDetail.BillNo;
                ibd.ProductCode = outBillDetail.ProductCode;
                ibd.UnitCode = outBillDetail.UnitCode;
                ibd.Price = outBillDetail.Price;
                ibd.BillQuantity = outBillDetail.BillQuantity * unit.Count;
                ibd.AllotQuantity = 0;
                ibd.RealQuantity = 0;
                ibd.Description = outBillDetail.Description;

                OutBillDetailRepository.Add(ibd);
                OutBillDetailRepository.SaveChanges();
            }
            else
            {
                isExistProduct.BillQuantity = isExistProduct.BillQuantity + (outBillDetail.BillQuantity * unit.Count);
                OutBillDetailRepository.SaveChanges();
            }
            return true;
        }

        public bool Delete(string ID)
        {
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            int id = Convert.ToInt32(ID);
            var outBillDetail = outBillDetailQuery.FirstOrDefault(o => o.ID == id);
            if (outBillDetail != null)
            {
                OutBillDetailRepository.Delete(outBillDetail);
                OutBillDetailRepository.SaveChanges();
            }
            return true;
        }

        public bool Save(OutBillDetail outBillDetail)
        {
            bool result = false;
            var outbm = OutBillDetailRepository.GetQueryable().FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ID == outBillDetail.ID);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == outBillDetail.UnitCode);
            if (outbm != null)
            {
                outbm.BillNo = outBillDetail.BillNo;
                outbm.ProductCode = outBillDetail.ProductCode;
                outbm.UnitCode = outBillDetail.UnitCode;
                outbm.Price = outBillDetail.Price;
                outbm.BillQuantity = outBillDetail.BillQuantity * unit.Count;
                outbm.AllotQuantity = 0;
                outbm.RealQuantity = 0;
                outbm.Description = outBillDetail.Description;

                OutBillDetailRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        #endregion
    }
}
