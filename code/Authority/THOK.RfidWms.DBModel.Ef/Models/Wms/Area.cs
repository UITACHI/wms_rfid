using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
   public class Area
    {
       public Area()
       {
           this.Shelfs = new List<Shelf>();
           this.Cells = new List<Cell>();
       }

       public string AreaCode { get; set; }
       public string AreaName { get; set; }
       public string ShortName { get; set; }
       public string AreaType { get; set; }
       public string WarehouseCode { get; set; }
       public string Description { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }

       public virtual Warehouse warehouse { get; set; }

       public virtual ICollection<Shelf> Shelfs { get; set; }
       public virtual ICollection<Cell> Cells { get; set; }
    }
}
