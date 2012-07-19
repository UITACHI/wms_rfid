using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class BillType
    {
        public BillType()
        {
            this.InBillMasters=new List<InBillMaster>();
            this.OutBillMasters = new List<OutBillMaster>();
            this.MoveBillMasters = new List<MoveBillMaster>();
            this.CheckBillMasters = new List<CheckBillMaster>();
            this.ProfitLossBillMasters = new List<ProfitLossBillMaster>();
        }
        public string BillTypeCode { get; set; }
        public string BillTypeName { get; set; }
        public string BillClass { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<InBillMaster> InBillMasters { get; set; }
        public virtual ICollection<OutBillMaster> OutBillMasters { get; set; }
        public virtual ICollection<MoveBillMaster> MoveBillMasters { get; set; }
        public virtual ICollection<CheckBillMaster> CheckBillMasters { get; set; }
        public virtual ICollection<ProfitLossBillMaster> ProfitLossBillMasters { get; set; }
    }
}
