using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IDepartmentService : IService<Department>
    {
        object GetDetails(int page, int rows, string DepartmentCode, string DepartmentName, string DepartmentLeaderID, string CompanyID);

        bool Add(Department department);

        bool Delete(string departmentId);

        bool Save(Department department);
    }
}
