using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Department
    {
        public Department()
        {
            this.Departments = new List<Department>();
            this.Employees = new List<Employee>();
        }

        public Guid ID { get; set; }
        public string DepartmentCode { get; set; }
        public string DepartmentName { get; set; }
        public Guid? DepartmentLeaderID { get; set; }
        public string Description { get; set; }
        public Guid CompanyID { get; set; }
        public Guid ParentDepartmentID { get; set; }
        public string UniformCode { get; set; }

        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Employee DepartmentLeader { get; set; }
        public virtual Company Company { get; set; }
        public virtual Department ParentDepartment { get; set; }

        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
