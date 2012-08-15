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
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

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
                                                      .OrderBy(i => i.BillNo).Select(i => i);
                int total = outBillDetail.Count();
                outBillDetail = outBillDetail.Skip((page - 1) * rows).Take(rows);

                var temp = outBillDetail.ToArray().AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.UnitCode,
                    i.Unit.UnitName,
                    BillQuantity = i.BillQuantity / i.Unit.Count,
                    AllotQuantity = i.AllotQuantity / i.Unit.Count,
                    RealQuantity = i.RealQuantity / i.Unit.Count,
                    i.Price,
                    i.Description
                });
                return new { total, rows = temp.ToArray() };
            }
            return "";
        }

        public bool Add(OutBillDetail outBillDetail, out string errorInfo)
        {
            errorInfo = string.Empty;
            bool result = false;
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            var isExistProduct = outBillDetailQuery.FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ProductCode == outBillDetail.ProductCode);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == outBillDetail.UnitCode);
            var storage = StorageRepository.GetQueryable().Where(s => s.ProductCode == outBillDetail.ProductCode);
            var storageQuantity = storage.Sum(s => (s.Quantity - s.OutFrozenQuantity));

            if (isExistProduct == null)
            {
                if (storageQuantity >= (outBillDetail.BillQuantity * unit.Count))
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
                    result = true;
                }
                else
                    errorInfo = "当前库存小于您输入的数量！请从新输入！";
            }
            else
            {
                if (storageQuantity >= isExistProduct.BillQuantity + (outBillDetail.BillQuantity * unit.Count))
                {
                    isExistProduct.BillQuantity = isExistProduct.BillQuantity + (outBillDetail.BillQuantity * unit.Count);
                    isExistProduct.UnitCode = outBillDetail.UnitCode;
                    OutBillDetailRepository.SaveChanges();
                    result = true;
                }
                else
                    errorInfo = "当前库存小于您输入的数量！请从新输入！";
            }
            return result;
        }

        public bool Delete(string ID, out string errorInfo)
        {
            errorInfo = string.Empty;
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

        public bool Save(OutBillDetail outBillDetail, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var outbm = OutBillDetailRepository.GetQueryable().FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ProductCode == outBillDetail.ProductCode);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == outBillDetail.UnitCode);
            var storage = StorageRepository.GetQueryable().Where(s => s.ProductCode == outBillDetail.ProductCode);//
            var storageQuantity = storage.Sum(s => (s.Quantity - s.OutFrozenQuantity));

            if ((outbm != null && outbm.ID == outBillDetail.ID)||outbm==null)
            {
                if (outbm == null)
                {
                    outbm = OutBillDetailRepository.GetQueryable().FirstOrDefault(i => i.BillNo == outBillDetail.BillNo && i.ID == outBillDetail.ID);
                }
                if (storageQuantity >= (outBillDetail.BillQuantity * unit.Count))
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
                else
                    errorInfo = "当前库存小于您输入的数量！请从新输入！";
            }
            else if (outbm != null && outbm.ID != outBillDetail.ID)
            {
                string err = string.Empty;
                if (storageQuantity >= outbm.BillQuantity + (outBillDetail.BillQuantity * unit.Count))
                {
                    bool deltrue = this.Delete(outBillDetail.ID.ToString(), out err);
                    outbm.BillNo = outBillDetail.BillNo;
                    outbm.ProductCode = outBillDetail.ProductCode;
                    outbm.UnitCode = outBillDetail.UnitCode;
                    outbm.Price = outBillDetail.Price;
                    outbm.BillQuantity = outbm.BillQuantity + (outBillDetail.BillQuantity * unit.Count);
                    outbm.Description = outBillDetail.Description;
                    OutBillDetailRepository.SaveChanges();
                    result = true;
                }
                else
                    errorInfo = "当前库存小于您输入的数量！请从新输入！";
            }
            
            return result;
        }

        #endregion
    }
}
