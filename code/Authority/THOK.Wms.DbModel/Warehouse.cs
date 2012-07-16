using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Warehouse
    {
        public Warehouse()
        {
            this.Areas = new List<Area>();
            this.Shelfs = new List<Shelf>();
            this.Cells = new List<Cell>();
        }

        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string ShortName { get; set; }
        public string WarehouseType { get; set; }
        public string CompanyCode { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Shelf> Shelfs { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
    }
}
