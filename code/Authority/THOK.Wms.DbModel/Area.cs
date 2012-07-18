using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
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
       public int AllotInOrder { get; set; }
       public int AllotOutOrder { get; set; }
       public string WarehouseCode { get; set; }
       public string Description { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }

       public virtual Warehouse Warehouse { get; set; }

       public virtual ICollection<Shelf> Shelfs { get; set; }
       public virtual ICollection<Cell> Cells { get; set; }
    }
}
