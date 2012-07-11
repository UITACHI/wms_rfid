using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IBrandService:IService<Brand>
    {
        object GetDetails(int page, int rows, string BrandCode, string BrandName, string IsActive);

        bool Add(Brand brand);

        bool Delete(string BrandCode);

        bool Save(Brand brand);
    }
}
