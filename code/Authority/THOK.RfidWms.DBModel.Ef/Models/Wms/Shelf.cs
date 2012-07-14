using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
   public class Shelf
    {
       public Shelf()
       {
           this.Cells = new List<Cell>();
       }

       public string ShelfCode { get; set; }
       public string ShelfName { get; set; }
       public string ShortName { get; set; }
       public string ShelfType { get; set; }
       public string WarehouseCode { get; set; }
       public string AreaCode { get; set; }
       public string Description { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }

       public virtual Area area { get; set; }
       public virtual Warehouse warehouse { get; set; }

       public virtual ICollection<Cell> Cells { get; set; }
    }
}
