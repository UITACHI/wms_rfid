using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class Employee
    {
        public Employee()
        {
            this.LeadDepartments = new List<Department>();
        }

        public Guid ID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Description { get; set; }
        public Guid? DepartmentID { get; set; }
        public Guid JobID { get; set; }
        public string Sex { get; set; }
        public string Tel { get; set; }
        public string Status { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Department Department { get; set; }
        public virtual Job Job { get; set; }

        public virtual ICollection<Department> LeadDepartments { get; set; }
    }
}
