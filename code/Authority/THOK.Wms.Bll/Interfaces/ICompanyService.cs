using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICompanyService : IService<Company>
    {
        object GetDetails(int page, int rows, string CompanyCode, string CompanyName, string CompanyType, string IsActive);

        bool Add(Company company);

        bool Delete(string companyID);

        bool Save(Company company);
        
    }
}
