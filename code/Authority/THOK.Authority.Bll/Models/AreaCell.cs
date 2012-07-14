using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Bll.Models
{
   public class AreaCell
    {
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string ShelfCode { get; set; }
        public string ShelfName { get; set; }
        public string CellCode { get; set; }
        public string CellName { get; set; }

        public int Layer { get; set; }
        public string Rfid { get; set; } 
        public string Type { get; set; }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public string UpdateTime { get; set; }
        public string DefaultProductCode { get; set; }
        public string isType { get; set; }

        public AreaCell[] children { get; set; }
    }
}
