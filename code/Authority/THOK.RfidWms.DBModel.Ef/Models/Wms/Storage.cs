using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
   public class Storage
    {
       public Storage()
       { 
            
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

       public virtual Product product { get; set; }
       public virtual Cell cell { get; set; }

    }
}
