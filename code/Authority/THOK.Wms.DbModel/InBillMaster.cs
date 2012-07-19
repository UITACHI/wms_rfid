using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class InBillMaster
    {
        public InBillMaster()
        {
            this.InBillDetails = new List<InBillDetail>();
            this.InBillAllots = new List<InBillAllot>();
        }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string BillTypeCode { get; set; }
        public string WarehouseCode { get; set; }
        public string OperatePersonCode { get; set; }
        public string Status { get; set; }
        public string VerifyPersonCode { get; set; }
        public DateTime ?VerifyDate { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual BillType BillType { get; set; }

        public virtual ICollection<InBillDetail> InBillDetails { get; set; }
        public virtual ICollection<InBillAllot> InBillAllots { get; set; }
    }
}
