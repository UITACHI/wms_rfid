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
    public class EmployeeService : ServiceBase<Employee>, IEmployeeService
    {
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }

        [Dependency]
        public IJobRepository JobRepository { get; set; }

        [Dependency]
        public IDepartmentRepository DepartmentRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IEmployeeService 成员

        public object GetDetails(int page, int rows, string EmployeeCode, string EmployeeName, string DepartmentID, string Status, string IsActive)
        {
            IQueryable<Employee> employeeQuery = EmployeeRepository.GetQueryable();
            var employee = employeeQuery.Where(e => e.EmployeeCode.Contains(EmployeeCode) && e.EmployeeName.Contains(EmployeeName)
                             && e.Status.Contains(Status))
                            .OrderBy(e => e.EmployeeCode).Select(e => new { e.ID, e.EmployeeCode, e.EmployeeName, DepartmentID = e.Department.ID, DepartmentName = e.Department.DepartmentName, e.Description, JobID = e.Job.ID, JobName = e.Job.JobName, e.Sex, e.Tel, e.Status, IsActive = e.IsActive == "1" ? "可用" : "不可用", e.UpdateTime });
            if (!DepartmentID.Equals(""))
            {
                Guid departID = new Guid(DepartmentID);
                employee = employeeQuery.Where(e => e.EmployeeCode.Contains(EmployeeCode) && e.EmployeeName.Contains(EmployeeName)
                             &&e.DepartmentID==departID && e.Status.Contains(Status))
                            .OrderBy(e => e.EmployeeCode).Select(e => new { e.ID, e.EmployeeCode, e.EmployeeName, DepartmentID = e.Department.ID, DepartmentName = e.Department.DepartmentName, e.Description, JobID = e.Job.ID, JobName = e.Job.JobName, e.Sex, e.Tel, e.Status, IsActive = e.IsActive == "1" ? "可用" : "不可用", e.UpdateTime });
            }
            int total = employee.Count();
            employee = employee.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = employee.ToArray() };
        }

        public bool Add(Employee employee)
        {
            var emp = new Employee();
            var job = JobRepository.GetQueryable().FirstOrDefault(j => j.ID == employee.JobID);
            var department =DepartmentRepository.GetQueryable().FirstOrDefault(d=>d.ID==employee.DepartmentID);
            emp.ID = Guid.NewGuid();
            emp.EmployeeCode = employee.EmployeeCode;
            emp.EmployeeName = employee.EmployeeName;
            emp.Description = employee.Description;
            //emp.Department = department;
            emp.Job = job;
            emp.Sex = employee.Sex;
            emp.Tel = employee.Tel;
            emp.Status = employee.Status;
            emp.IsActive = employee.IsActive;
            emp.UpdateTime = DateTime.Now;

            EmployeeRepository.Add(emp);
            EmployeeRepository.SaveChanges();
            return true;
        }

        public bool Delete(string employeeId)
        {
            Guid empId = new Guid(employeeId);
            var employee = EmployeeRepository.GetQueryable()
                .FirstOrDefault(e => e.ID == empId);
            if (employee != null)
            {
                EmployeeRepository.Delete(employee);
                EmployeeRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Employee employee)
        {
            var emp = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.ID == employee.ID);
            var department = DepartmentRepository.GetQueryable().FirstOrDefault(d => d.ID == employee.DepartmentID);
            var job = JobRepository.GetQueryable().FirstOrDefault(j => j.ID == employee.JobID);
            emp.EmployeeCode = employee.EmployeeCode;
            emp.EmployeeName = employee.EmployeeName;
            emp.Description = employee.Description;
            emp.Department = department;
            emp.Job = job;
            emp.Sex = employee.Sex;
            emp.Tel = employee.Tel;
            emp.Status = employee.Status;
            emp.IsActive = employee.IsActive;
            emp.UpdateTime = DateTime.Now;

            EmployeeRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
