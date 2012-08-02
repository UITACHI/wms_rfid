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
            this.OperatePersonMoveBillMasters = new List<MoveBillMaster>();
            this.VerifyPersonMoveBillMasters = new List<MoveBillMaster>();
            this.MoveBillDetails = new List<MoveBillDetail>();
            this.OperatePersonCheckBillMasters = new List<CheckBillMaster>();
            this.VerifyPersonCheckBillMasters = new List<CheckBillMaster>();
            this.CheckBillDetails = new List<CheckBillDetail>();
            this.OperatePersonOutBillMasters = new List<OutBillMaster>();
            this.VerifyPersonOutBillMasters = new List<OutBillMaster>();
            this.OperatePersonInBillMasters = new List<InBillMaster>();
            this.VerifyPersonInBillMasters = new List<InBillMaster>();
            this.OperatePersonProfitLossBillMasters = new List<ProfitLossBillMaster>();
            this.VerifyPersonProfitLossBillMasters = new List<ProfitLossBillMaster>();
        }

        public Guid ID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
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
        public virtual ICollection<MoveBillMaster> OperatePersonMoveBillMasters { get; set; }
        public virtual ICollection<MoveBillMaster> VerifyPersonMoveBillMasters { get; set; }
        public virtual ICollection<MoveBillDetail> MoveBillDetails { get; set; }
        public virtual ICollection<CheckBillMaster> OperatePersonCheckBillMasters { get; set; }
        public virtual ICollection<CheckBillMaster> VerifyPersonCheckBillMasters { get; set; }
        public virtual ICollection<ProfitLossBillMaster> OperatePersonProfitLossBillMasters { get; set; }
        public virtual ICollection<ProfitLossBillMaster> VerifyPersonProfitLossBillMasters { get; set; }
        public virtual ICollection<CheckBillDetail> CheckBillDetails { get; set; }
        public virtual ICollection<OutBillMaster> OperatePersonOutBillMasters { get; set; }
        public virtual ICollection<OutBillMaster> VerifyPersonOutBillMasters { get; set; }
        public virtual ICollection<InBillMaster> OperatePersonInBillMasters { get; set; }
        public virtual ICollection<InBillMaster> VerifyPersonInBillMasters { get; set; }

    }
}
