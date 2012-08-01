using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class InBillAllot
    {
        public InBillAllot()
        {
        }
        public int ID { get; set; }
        public string BillNo { get; set; }
        public string ProductCode { get; set; }
        public int InBillDetailId { get; set; }
        public string CellCode { get; set; }
        public string StorageCode { get; set; }
        public string UnitCode { get; set; }
        public decimal AllotQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public Guid ?OperatePersonID { get; set; }
        public DateTime ?StartTime { get; set; }
        public DateTime ?FinishTime { get; set; }
        public string Status { get; set; }

        public virtual InBillMaster InBillMaster { get; set; }
        public virtual InBillDetail InBillDetail { get; set; }
        public virtual Product Product { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual Cell Cell { get; set; }
        public virtual Unit Unit { get; set; }

    }
}
