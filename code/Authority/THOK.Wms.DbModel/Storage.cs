using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
   public class Storage
    {
       public Storage()
       {
           this.MoveBillDetailOutStorages = new List<MoveBillDetail>();
           this.MoveBillDetailInStorages = new List<MoveBillDetail>();
           this.CheckBillDetails = new List<CheckBillDetail>();
           this.InBillAllots = new List<InBillAllot>();
           this.OutBillAllots = new List<OutBillAllot>();
           this.ProfitLossBillDetails = new List<ProfitLossBillDetail>();
       }
       public string StorageCode { get; set; }
       public string CellCode { get; set; }
       public string ProductCode { get; set; }
       public decimal Quantity { get; set; }
       public DateTime StorageTime { get; set; }
       public string Rfid { get; set; }
       public decimal InFrozenQuantity { get; set; }
       public decimal OutFrozenQuantity { get; set; }
       public string IsLock { get; set; }
       public string LockTag { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }
       public byte[] RowVersion { get; set; }

       public virtual Product Product { get; set; }
       public virtual Cell Cell { get; set; }

       public virtual ICollection<MoveBillDetail> MoveBillDetailOutStorages { get; set; }
       public virtual ICollection<MoveBillDetail> MoveBillDetailInStorages { get; set; }
       public virtual ICollection<CheckBillDetail> CheckBillDetails { get; set; }
       public virtual ICollection<InBillAllot> InBillAllots { get; set; }
       public virtual ICollection<OutBillAllot> OutBillAllots { get; set; }
       public virtual ICollection<ProfitLossBillDetail> ProfitLossBillDetails { get; set; }

    }
}
