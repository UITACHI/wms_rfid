using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IBrandService:IService<Brand>
    {
        object GetDetails(int page, int rows, string BrandCode, string BrandName, string IsActive);

        bool Add(Brand brand);

        bool Delete(string BrandCode);

        bool Save(Brand brand);
        
    }
}
