using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class Cell
    {
        public Cell()
        {
            this.Storage = new List<Storage>();
        }
        public string CellCode { get; set; }
        public string CellName { get; set; }
        public string ShortName { get; set; }
        public string CellType { get; set; }
        public int Layer { get; set; }
        public string Rfid { get; set; }
        public string WarehouseCode { get; set; }
        public string AreaCode { get; set; }
        public string ShelfCode { get; set; }
        public string DefaultProductCode { get; set; }
        public int MaxQuantity { get; set; }
        public string IsSingle { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Warehouse warehouse { get; set; }
        public virtual Area area { get; set; }
        public virtual Shelf shelf { get; set; }
        public virtual Product product { get; set; }

        public virtual ICollection<Storage> Storage { get; set; }
    }
}
