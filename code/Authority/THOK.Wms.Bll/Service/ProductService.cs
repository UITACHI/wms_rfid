using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class ProductService:ServiceBase<Product>,IProductService
    {
        [Dependency]
        public IProductRepository ProductRepository { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IProductService 增，删，改，查等方法

        public object GetDetails(int page, int rows, string ProductName, string ProductCode, string CustomCode, string BrandCode, string UniformCode, string AbcTypeCode, string ShortCode, string PriceLevelCode, string SupplierCode)
        {
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.Where(c => c.ProductName.Contains(ProductName) && c.ProductCode.Contains(ProductCode)
                && c.CustomCode.Contains(CustomCode) && c.BrandCode.Contains(BrandCode) && c.UniformCode.Contains(UniformCode)
                && c.AbcTypeCode.Contains(AbcTypeCode) && c.ShortCode.Contains(ShortCode) && c.PriceLevelCode.Contains(PriceLevelCode) && c.SupplierCode.Contains(SupplierCode))
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
                    UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
                });
            int total = product.Count();
            product = product.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = product.ToArray() };
        }
        public bool Add(Product product)
        {
            var prod = new Product();
            prod.AbcTypeCode = product.AbcTypeCode;
            prod.BarBarcode = product.BarBarcode;
            prod.BelongRegion = product.BelongRegion;
            prod.BrandCode = product.BrandCode;

            prod.BuyPrice = product.BuyPrice;
            prod.CostPrice = product.CostPrice;
            prod.CustomCode = product.CustomCode;
            prod.Description = product.Description;
            prod.IsAbnormity = product.IsAbnormity;
            prod.IsActive = product.IsActive;
            prod.IsConfiscate = product.IsConfiscate;
            prod.IsFamous = product.IsFamous;
            prod.IsFilterTip = product.IsFilterTip;
            prod.IsMainProduct = product.IsMainProduct;
            prod.IsNew = product.IsNew;
            prod.IsProvinceMainProduct = product.IsProvinceMainProduct;
            prod.OneProjectBarcode = product.OneProjectBarcode;
            prod.PackageBarcode = product.PackageBarcode;
            prod.PackTypeCode = product.PackTypeCode;
            prod.PieceBarcode = product.PieceBarcode;
            prod.PriceLevelCode = product.PriceLevelCode;
            prod.ProductCode = product.ProductCode;
            prod.ProductName = product.ProductName;
            prod.ProductTypeCode = product.ProductTypeCode;
            prod.RetailPrice = product.RetailPrice;
            prod.ShortCode = product.ShortCode;
            prod.StatisticType = product.StatisticType;
            prod.SupplierCode = product.SupplierCode;
            prod.TradePrice = product.TradePrice;
            prod.UniformCode = product.UniformCode;
            prod.UnitCode = product.UnitCode;
            prod.UnitListCode = product.UnitListCode;
            prod.UpdateTime = DateTime.Now;
       



            ProductRepository.Add(prod);
            ProductRepository.SaveChanges();
            return true;
        }
        public bool Delete(string ProductCode)
        {
            var product = ProductRepository.GetQueryable()
                .FirstOrDefault(b => b.ProductCode == ProductCode);
            if (ProductCode != null)
            {
                ProductRepository.Delete(product);
                ProductRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }
        public bool Save(Product product)
        {
            var prod = ProductRepository.GetQueryable().FirstOrDefault(b => b.ProductCode == product.ProductCode);
            prod.AbcTypeCode = product.AbcTypeCode;
            prod.BarBarcode = product.BarBarcode;
            prod.BelongRegion = product.BelongRegion;
            prod.BrandCode = product.BrandCode;
            prod.BuyPrice = product.BuyPrice;
            prod.CostPrice = product.CostPrice;
            prod.CustomCode = product.CustomCode;
            prod.Description = product.Description;
            prod.IsAbnormity = product.IsAbnormity;
            prod.IsActive = product.IsActive;
            prod.IsConfiscate = product.IsConfiscate;
            prod.IsFamous = product.IsFamous;
            prod.IsFilterTip = product.IsFilterTip;
            prod.IsMainProduct = product.IsMainProduct;
            prod.IsNew = product.IsNew;
            prod.IsProvinceMainProduct = product.IsProvinceMainProduct;
            prod.OneProjectBarcode = product.OneProjectBarcode;
            prod.PackageBarcode = product.PackageBarcode;
            prod.PackTypeCode = product.PackTypeCode;
            prod.PieceBarcode = product.PieceBarcode;
            prod.PriceLevelCode = product.PriceLevelCode;
            prod.ProductCode = product.ProductCode;
            prod.ProductName = product.ProductName;
            prod.ProductTypeCode = product.ProductTypeCode;
            prod.RetailPrice = product.RetailPrice;
            prod.ShortCode = product.ShortCode;
            prod.StatisticType = product.StatisticType;
            prod.SupplierCode = product.SupplierCode;
            prod.TradePrice = product.TradePrice;
            prod.UniformCode = product.UniformCode;
            prod.UnitCode = product.UnitCode;
            prod.UnitListCode = product.UnitListCode;
            prod.UpdateTime = DateTime.Now;

            ProductRepository.SaveChanges();
            return true;
        }
        #endregion

        #region IProductService 成员

        /// <summary>
        /// 查询卷烟信息 zxl 2012年7月24日 16:26:24
        /// </summary>
        /// <returns></returns>
        public object FindProduct()
        {
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.OrderBy(p => p.ProductCode).Select(p => new { p.ProductCode, p.ProductName });
            return product.ToArray();
        }

        /// <summary>浏览加载卷烟信息</summary>
        public object LoadProduct(int page, int rows)
        {
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.OrderBy(p => p.ProductCode).Select(p => new { p.ProductCode, p.ProductName });
            int total = product.Count();
            product = product.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = product.ToArray() };
        }
        

        /// <summary>获取卷烟信息</summary>
        public object GetProductBy(int page, int rows, string QueryString, string Value)
        {
            string productCode = "", productName = "";
            
            if (QueryString == "ProductCode")
            {
                productCode = Value;
            }
            else
            {
                productName = Value;
            }
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.Where(c => c.ProductCode.Contains(productCode) && c.ProductName.Contains(productName))
                .OrderBy(c => c.ProductCode).AsEnumerable()
                .Select(c => new { 
                    c.ProductCode,
                    c.ProductName
                });
            int total = product.Count();
            product = product.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = product.ToArray() };
        }

        #endregion
    }
}
