using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
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

        public object GetDetails(int page, int rows, string EmployeeCode, string EmployeeName, string DepartmentID, string JobID, string Status, string IsActive)
        {
            IQueryable<Employee> employeeQuery = EmployeeRepository.GetQueryable();
            var employee = employeeQuery.Where(e => e.EmployeeCode.Contains(EmployeeCode) && e.EmployeeName.Contains(EmployeeName)
                             && e.Status.Contains(Status) && e.IsActive.Contains(IsActive));
                            
            if (!DepartmentID.Equals(string.Empty))
            {
                Guid departID = new Guid(DepartmentID);
                employee = employee.Where(e => e.DepartmentID == departID);
            }
            if (!JobID.Equals(string.Empty))
            {
                Guid jobID = new Guid(JobID);
                employee = employee.Where(e => e.JobID == jobID);
            }

            var temp = employee.AsEnumerable().Select(e => new
            {
                e.ID,
                e.EmployeeCode,
                e.EmployeeName,
                DepartmentID=e.DepartmentID== null ? string.Empty :e.DepartmentID.ToString(),
                DepartmentName =e.DepartmentID == null ? string.Empty : e.Department.DepartmentName,
                e.Description,
                JobID =e.Job==null?string.Empty:e.Job.ID.ToString(),
                JobName = e.Job == null ? string.Empty : e.Job.JobName,
                e.Sex,
                e.Tel,
                e.Status,
                IsActive = e.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = e.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
            });
            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
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
            emp.Department = department;
            emp.Job = job;
            emp.Sex = employee.Sex;
            emp.Tel = employee.Tel;
            emp.Status = employee.Status;
            emp.IsActive = employee.IsActive;
            emp.UserName = employee.UserName;
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
            emp.UserName = employee.UserName;
            emp.UpdateTime = DateTime.Now;

            EmployeeRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
