using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Employee
    {
        public Employee()
        {
            this.LeadDepartments = new List<Department>();
            this.MoveBillMasterOperatePersons = new List<MoveBillMaster>();
            this.MoveBillMasterVerifyPersons = new List<MoveBillMaster>();
            this.MoveBillDetails = new List<MoveBillDetail>();
            this.CheckBillMasterOperatePersons = new List<CheckBillMaster>();
            this.CheckBillMasterVerifyPersons = new List<CheckBillMaster>();
            this.CheckBillDetails = new List<CheckBillDetail>();
            this.OutBillMasterOperatePersons = new List<OutBillMaster>();
            this.OutBillMasterVerifyPersons = new List<OutBillMaster>();
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
        public virtual ICollection<MoveBillMaster> MoveBillMasterOperatePersons { get; set; }
        public virtual ICollection<MoveBillMaster> MoveBillMasterVerifyPersons { get; set; }
        public virtual ICollection<MoveBillDetail> MoveBillDetails { get; set; }
        public virtual ICollection<CheckBillMaster> CheckBillMasterOperatePersons { get; set; }
        public virtual ICollection<CheckBillMaster> CheckBillMasterVerifyPersons { get; set; }
        public virtual ICollection<CheckBillDetail> CheckBillDetails { get; set; }
        public virtual ICollection<OutBillMaster> OutBillMasterOperatePersons { get; set; }
        public virtual ICollection<OutBillMaster> OutBillMasterVerifyPersons { get; set; }

    }
}
