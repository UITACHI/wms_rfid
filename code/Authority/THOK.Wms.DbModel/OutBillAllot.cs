using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class OutBillAllot
    {
        public OutBillAllot()
        {
        }
        public int ID { get; set; }
        public string BillNo { get; set; }
        public int OutPalletTag { get; set; }
        public string ProductCode { get; set; }
        public int OutBillDetailId { get; set; }
        public string CellCode { get; set; }
        public string StorageCode { get; set; }
        public string UnitCode { get; set; }
        public decimal AllotQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public Guid? OperatePersonID { get; set; }
        public DateTime ?StartTime { get; set; }
        public DateTime ?FinishTime { get; set; }
        public string Status { get; set; }

        public virtual OutBillMaster OutBillMaster { get; set; }
        public virtual OutBillDetail OutBillDetail { get; set; }
        public virtual Product Product { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual Cell Cell { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
