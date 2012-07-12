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

        public object GetDetails(int page, int rows, string CompanyCode)
        {
            IQueryable<Product> ProductQuery = ProductRepository.GetQueryable();
            var product = ProductQuery.Where(c => c.ProductName.Contains(CompanyCode))
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
                    c.UpdateTime
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
        #endregion
    }
}
