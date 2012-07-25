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
       }
       public string StorageCode { get; set; }
       public string CellCode { get; set; }
       public string ProductCode { get; set; }
       public int Quantity { get; set; }
       public DateTime StorageTime { get; set; }
       public string Rfid { get; set; }
       public int InFrozenQuantity { get; set; }
       public int OutFrozenQuantity { get; set; }
       public string IsLock { get; set; }
       public string LockTag { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }
       public byte[] RowVersion { get; set; }

       public virtual Product Products { get; set; }
       public virtual Cell Cells { get; set; }

       public virtual ICollection<MoveBillDetail> MoveBillDetailOutStorages { get; set; }
       public virtual ICollection<MoveBillDetail> MoveBillDetailInStorages { get; set; }
       public virtual ICollection<CheckBillDetail> CheckBillDetails { get; set; }

    }
}
