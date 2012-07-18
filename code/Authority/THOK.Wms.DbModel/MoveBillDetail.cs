using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
  public  class MoveBillDetail
    {
        public int ID { get; set; }
        public string BillNo { get; set; }
        public string ProductCode { get; set; }
        public string OutCellCode { get; set; }
        public string OutStorageCode { get; set; }
        public string InCellCode { get; set; }
        public string InStorageCode { get; set; }
        public string UnitCode { get; set; }
        public decimal RealQuantity { get; set; }
        public string OperatePersonCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Status { get; set; }

        public virtual MoveBillMaster MoveBillMaster { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Employee OperatePerson { get; set; }
        public virtual Cell OutCell { get; set; }
        public virtual Cell InCell { get; set; }
        public virtual Storage OutStorage { get; set; }
        public virtual Storage InStorage { get; set; }
    }
}
