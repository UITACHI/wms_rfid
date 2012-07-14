using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Bll.Interfaces.Wms;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces.Wms;

namespace THOK.Authority.Bll.Service.Wms
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

        public object GetDetails(int page, int rows, string productCode)
        {
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.Where(c => c.ProductName.Contains(productCode))
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
                    c.IsAbnormity,
                    c.IsActive,
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
                    c.UnitListCode,
                    UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
                    //ParentCompanyName = c.ParentCompany.CompanyName,
                    //c.ParentCompanyID,
                    //IsActive = c.IsActive == "1" ? "可用" : "不可用",
                    //UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
                });
            //if (!IsActive.Equals(""))
            //{
            //    string bStatus = IsActive == "可用" ? "1" : "0";
            //    company = companyQuery.Where(c => c.CompanyCode.Contains(CompanyCode) && c.CompanyName.Contains(CompanyName) && c.CompanyType.Contains(CompanyType) && c.IsActive.Contains(bStatus))
            //    .OrderBy(c => c.CompanyCode).AsEnumerable()
            //    .Select(c => new
            //    {
            //        c.ID,
            //        c.CompanyCode,
            //        c.CompanyName,
            //        c.Description,
            //        c.CompanyType,
            //        c.WarehouseCapacity,
            //        c.WarehouseCount,
            //        c.WarehouseSpace,
            //        c.SortingCount,
            //        ParentCompanyName = c.ParentCompany.CompanyName,
            //        c.ParentCompanyID,
            //        IsActive = c.IsActive == "1" ? "可用" : "不可用",
            //        UpdateTime = c.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
            //    });
            //}
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
            //prod.BrandCode = product.BrandCode;
            prod.BrandCode = "21332";
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
            //prod.SupplierCode = product.SupplierCode;
            prod.SupplierCode = "001";
            prod.TradePrice = product.TradePrice;
            prod.UniformCode = product.UniformCode;
            //prod.UnitCode = product.UnitCode;
            //prod.UnitListCode = product.UnitListCode;
            prod.UnitCode = "3324324";
            prod.UnitListCode = "3232";
            prod.UpdateTime = DateTime.Now;



            ProductRepository.Add(prod);
            ProductRepository.SaveChanges();
            return true;
        }
        public bool Delete(string ProductCode)
        {
            var brand = ProductRepository.GetQueryable()
                .FirstOrDefault(b => b.BrandCode == ProductCode);
            if (ProductCode != null)
            {
                ProductRepository.Delete(brand);
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
            //prod.BrandCode = product.BrandCode;
            prod.BrandCode = "21332";
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
            //prod.SupplierCode = product.SupplierCode;
            prod.SupplierCode = "11111";
            prod.TradePrice = product.TradePrice;
            prod.UniformCode = product.UniformCode;
            //prod.UnitCode = product.UnitCode;
            //prod.UnitListCode = product.UnitListCode;
            prod.UnitCode = "3324324";
            prod.UnitListCode = "3232";
            prod.UpdateTime = DateTime.Now;

            ProductRepository.SaveChanges();
            return true;
        }
        #endregion
    }
}
