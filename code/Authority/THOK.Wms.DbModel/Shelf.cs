using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
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
       public int CellRows { get; set; }
       public int CellCols { get; set; }
       public int ImgX { get; set; }
       public int ImgY { get; set; }
       public string WarehouseCode { get; set; }
       public string AreaCode { get; set; }
       public string Description { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }

       public virtual Area Area { get; set; }
       public virtual Warehouse Warehouse { get; set; }

       public virtual ICollection<Cell> Cells { get; set; }
    }
}
