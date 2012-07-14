using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IProductService:IService<Product>
    {
        object GetDetails(int page, int rows, string ProductName, string ProductCode, string CustomCode, string BrandCode, string UniformCode, string AbcTypeCode, string ShortCode, string PriceLevelCode, string SupplierCode);
        bool Add(Product product);
        bool Delete(string ProductCode);
        bool Save(Product product);
    }
}
