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
    public class BrandService:ServiceBase<Brand>,IBrandService
    {
        [Dependency]
        public IBrandRepository BrandRepository { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IBrandService 成员

        public object GetDetails(int page, int rows, string BrandCode, string BrandName, string IsActive)
        {
            IQueryable<Brand> brandQuery = BrandRepository.GetQueryable();
            var brand = brandQuery.Where(b => b.BrandCode.Contains(BrandCode) && b.BrandName.Contains(BrandName)).OrderBy(b => b.BrandCode).AsEnumerable().Select(b => new { b.BrandCode, b.UniformCode, b.CustomCode, b.BrandName, b.SupplierCode, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                brand = brandQuery.Where(b => b.BrandCode.Contains(BrandCode) && b.BrandName.Contains(BrandName) && b.IsActive.Contains(IsActive)).OrderBy(b => b.BrandCode).AsEnumerable().Select(b => new { b.BrandCode, b.UniformCode, b.CustomCode, b.BrandName, b.SupplierCode, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            }
            int total = brand.Count();
            brand = brand.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = brand.ToArray() };
        }

        public new bool Add(Brand brand)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string BrandCode)
        {
            throw new NotImplementedException();
        }

        public bool Save(Brand brand)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
