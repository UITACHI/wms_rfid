using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
  public  class CheckBillDetail
    {
        public int ID { get; set; }
        public string BillNo { get; set; }
        public string CellCode { get; set; }
        public string StorageCode { get; set; }
        public string ProductCode { get; set; }
        public string UnitCode { get; set; }
        public decimal Quantity { get; set; }
        public string RealProductCode { get; set; }
        public string RealUnitCode { get; set; }
        public decimal RealQuantity { get; set; }
        public string OperatePersonCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Status { get; set; }

        public virtual CheckBillMaster CheckBillMaster { get; set; }
        public virtual Cell Cell { get; set; }
        public virtual Storage Storage { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Product RealProduct { get; set; }
        public virtual Unit RealUnit { get; set; }
        public virtual Employee OperatePerson { get; set; }
        
    }
}
