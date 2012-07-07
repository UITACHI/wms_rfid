using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IEmployeeService : IService<Employee>
    {
        object GetDetails(int page, int rows, string EmployeeCode, string EmployeeName, string DepartmentID, string Status, string IsActive);

        bool Add(Employee employee);

        bool Delete(string employeeId);

        bool Save(Employee employee);
    }
}
