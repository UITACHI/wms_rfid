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
    public class InBillDetailService:ServiceBase<InBillDetail>,IInBillDetailService
    {
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }
        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IInBillDetailService 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<InBillDetail> inBillDetailQuery = InBillDetailRepository.GetQueryable();
                var inBillDetail = inBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i =>i);
                int total = inBillDetail.Count();
                inBillDetail = inBillDetail.Skip((page - 1) * rows).Take(rows);

                var temp = inBillDetail.ToArray().AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.UnitCode,
                    i.Unit.UnitName,
                    BillQuantity = i.BillQuantity / i.Unit.Count,
                    RealQuantity = i.RealQuantity / i.Unit.Count,
                    AllotQuantity = i.AllotQuantity / i.Unit.Count,
                    i.Price,
                    i.Description
                });
                return new { total, rows = temp.ToArray() };
            }
            return "";
        }

        public new bool Add(InBillDetail inBillDetail)
        {
            IQueryable<InBillDetail> inBillDetailQuery = InBillDetailRepository.GetQueryable();
            var isExistProduct = inBillDetailQuery.FirstOrDefault(i=>i.BillNo==inBillDetail.BillNo&&i.ProductCode==inBillDetail.ProductCode);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == inBillDetail.UnitCode);
            if (isExistProduct == null)
            {
                var ibd = new InBillDetail();
                ibd.BillNo = inBillDetail.BillNo;
                ibd.ProductCode = inBillDetail.ProductCode;
                ibd.UnitCode = inBillDetail.UnitCode;
                ibd.Price = inBillDetail.Price;
                ibd.BillQuantity = inBillDetail.BillQuantity*unit.Count;
                ibd.AllotQuantity = 0;
                ibd.RealQuantity = 0;
                ibd.Description = inBillDetail.Description;

                InBillDetailRepository.Add(ibd);
                InBillDetailRepository.SaveChanges();
            }
            else
            {
                var ibd = inBillDetailQuery.FirstOrDefault(i => i.BillNo == inBillDetail.BillNo && i.ProductCode == inBillDetail.ProductCode);
                ibd.UnitCode = inBillDetail.UnitCode;
                ibd.BillQuantity = ibd.BillQuantity + inBillDetail.BillQuantity*unit.Count;
                InBillDetailRepository.SaveChanges();
            }
            return true;
        }

        public bool Delete(string ID)
        {
            IQueryable<InBillDetail> inBillDetailQuery = InBillDetailRepository.GetQueryable();
            int intID = Convert.ToInt32(ID);
            var ibd = inBillDetailQuery.FirstOrDefault(i=>i.ID==intID);
            InBillDetailRepository.Delete(ibd);
            InBillDetailRepository.SaveChanges();
            return true;
        }

        public bool Save(InBillDetail inBillDetail)
        {
            bool result=false;
            IQueryable<InBillDetail> inBillDetailQuery = InBillDetailRepository.GetQueryable();
            var ibd = inBillDetailQuery.FirstOrDefault(i=>i.BillNo==inBillDetail.BillNo&&inBillDetail.ProductCode==inBillDetail.ProductCode);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == inBillDetail.UnitCode);
            if ((ibd!= null&&ibd.ID==inBillDetail.ID)||ibd==null)
            {
                if(ibd==null)
                {
                    ibd=inBillDetailQuery.FirstOrDefault(i=>i.BillNo==inBillDetail.BillNo&&i.ID==inBillDetail.ID);
                }
                ibd.BillNo=inBillDetail.BillNo;
                ibd.ProductCode = inBillDetail.ProductCode;
                ibd.UnitCode = inBillDetail.UnitCode;
                ibd.Price = inBillDetail.Price;
                ibd.BillQuantity = inBillDetail.BillQuantity * unit.Count;
                ibd.Description = inBillDetail.Description;
                InBillDetailRepository.SaveChanges();
                result = true;
            }
            else if(ibd != null && ibd.ID != inBillDetail.ID)
            {
                bool delDetail=this.Delete(inBillDetail.ID.ToString());
                ibd.BillNo=inBillDetail.BillNo;
                ibd.ProductCode=inBillDetail.ProductCode;
                ibd.UnitCode=inBillDetail.UnitCode;
                ibd.Price=inBillDetail.Price;
                ibd.BillQuantity=ibd.BillQuantity+inBillDetail.BillQuantity*unit.Count;
                ibd.Description=inBillDetail.Description;
                InBillDetailRepository.SaveChanges();
                result=true;
            }
            return result;
        }

        #endregion

        #region IInBillDetailService 成员


        public object GetProductDetails(int page, int rows, string QueryString, string Value)
        {
            string ProductName = "";
            string ProductCode = "";
            if (QueryString == "ProductCode")
            {
                ProductCode = Value;
            }
            else
            {
                ProductName = Value;
            }
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.Where(c => c.ProductName.Contains(ProductName) && c.ProductCode.Contains(ProductCode)&& c.IsActive=="1")
                .OrderBy(c => c.ProductCode).AsEnumerable()
                .Select(c => new
                {
                    c.AbcTypeCode,
                    c.BarBarcode,
                    c.BelongRegion,
                    c.BrandCode,
                    c.BuyPrice,
                    c.CostPrice,
                    c.CustomCode,
                    c.Description,
                    IsAbnormity = c.IsAbnormity == "1" ? "是" : "不是",
                    IsActive = c.IsActive == "1" ? "可用" : "不可用",
                    c.IsConfiscate,
                    c.IsFamous,
                    c.IsFilterTip,
                    c.IsMainProduct,
                    c.IsNew,
                    c.IsProvinceMainProduct,
                    c.OneProjectBarcode,
                    c.PackageBarcode,
                    c.PackTypeCode,
                    c.PieceBarcode,
                    c.PriceLevelCode,
                    c.ProductCode,
                    c.ProductName,
                    c.ProductTypeCode,
                    c.RetailPrice,
                    c.ShortCode,
                    c.StatisticType,
                    c.SupplierCode,
                    c.TradePrice,
                    c.UniformCode,
                    c.UnitCode,
                    c.Unit.UnitName,
                    c.UnitListCode,
                    UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                });
            int total = product.Count();
            product = product.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = product.ToArray() };
        }

        #endregion
    }
}
