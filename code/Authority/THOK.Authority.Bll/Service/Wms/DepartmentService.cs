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
    public class DepartmentService : ServiceBase<Department>, IDepartmentService
    {
        [Dependency]
        public IDepartmentRepository DepartmentRepository { get; set; }

        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }

        [Dependency]
        public ICompanyRepository CompanyRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IDepartmentService 增，删，改，查等方法

        public object GetDetails(int page, int rows, string DepartmentCode, string DepartmentName, string DepartmentLeaderID, string CompanyID)
        {
            IQueryable<Department> departQuery = DepartmentRepository.GetQueryable();
            var department = departQuery.Where(d => d.DepartmentCode.Contains(DepartmentCode) && d.DepartmentName.Contains(DepartmentName))
                .OrderBy(d => d.DepartmentCode).Select(d => new { d.ID, d.DepartmentCode, d.DepartmentName, EmployeeID = d.DepartmentLeader.ID, d.DepartmentLeader.EmployeeName, d.Description, companyID = d.Company.ID, d.Company.CompanyName, ParentDepartmentID = d.DepartmentLeader.ID, ParentDepartmentName = d.ParentDepartment.DepartmentName, d.IsActive, d.UpdateTime });
            if (!CompanyID.Equals("") || !DepartmentLeaderID.Equals(""))
            {
                var compId = new Guid(CompanyID);
                var empId = new Guid(DepartmentLeaderID);
                department = departQuery.Where(d => d.DepartmentCode.Contains(DepartmentCode) && d.DepartmentName.Contains(DepartmentName)&& d.Company.ID==compId && d.DepartmentLeader.ID==empId)
                .OrderBy(d => d.DepartmentCode).Select(d => new { d.ID, d.DepartmentCode, d.DepartmentName, EmployeeID = d.DepartmentLeader.ID, d.DepartmentLeader.EmployeeName, d.Description, companyID = d.Company.ID, d.Company.CompanyName, ParentDepartmentID = d.DepartmentLeader.ID, ParentDepartmentName = d.ParentDepartment.DepartmentName, d.IsActive, d.UpdateTime });
            }
            int total = department.Count();
            department = department.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = department.ToArray() };
        }

        public bool Add(Department department)
        {
            var newDepartment = new Department();
            var depart = DepartmentRepository.GetQueryable().FirstOrDefault(d => d.ID == department.ParentDepartment.ID);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.ID == department.DepartmentLeaderID);
            var company = CompanyRepository.GetQueryable().FirstOrDefault(c => c.ID == department.CompanyID);
            newDepartment.ID = Guid.NewGuid();
            newDepartment.DepartmentCode = department.DepartmentCode;
            newDepartment.DepartmentName = department.DepartmentName;
            newDepartment.ParentDepartment = depart ?? newDepartment;
            newDepartment.DepartmentLeader = employee;
            newDepartment.Description = department.Description;
            newDepartment.Company = company;
            newDepartment.UniformCode = department.UniformCode;
            newDepartment.IsActive = department.IsActive;
            newDepartment.UpdateTime = DateTime.Now;

            DepartmentRepository.Add(newDepartment);
            DepartmentRepository.SaveChanges();
            return true;
        }

        public bool Delete(string departmentId)
        {
            Guid deparId = new Guid(departmentId);
            var departemnt = DepartmentRepository.GetQueryable()
                .FirstOrDefault(c => c.ID == deparId);
            if (departemnt != null)
            {
                //Del(DepartmentRepository, departemnt.Companies);
                DepartmentRepository.Delete(departemnt);
                DepartmentRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Department department)
        {
            var depart = DepartmentRepository.GetQueryable().FirstOrDefault(d => d.ID == department.ID);
            var parent = DepartmentRepository.GetQueryable().FirstOrDefault(p => p.ID == department.ParentDepartmentID);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.ID == department.DepartmentLeaderID);
            var company = CompanyRepository.GetQueryable().FirstOrDefault(c => c.ID == department.CompanyID);
            depart.DepartmentCode = department.DepartmentCode;
            depart.DepartmentName = department.DepartmentName;
            depart.ParentDepartment = depart ?? depart;
            depart.DepartmentLeader = employee;
            depart.Description = department.Description;
            depart.Company = company;
            depart.UniformCode = department.UniformCode;
            depart.IsActive = department.IsActive;
            depart.UpdateTime = DateTime.Now;
            DepartmentRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
