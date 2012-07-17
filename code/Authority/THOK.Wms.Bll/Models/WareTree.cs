using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.Bll.Models
{
    public class WareTree
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string ShelfCode { get; set; }
        public string ShelfName { get; set; }
        public string CellCode { get; set; }
        public string CellName { get; set; }

        public int AllotInOrder{ get; set; }
        public int AllotOutOrder { get; set; }

        public int MaxQuantity { get; set; }
        public int Layer { get; set; }
        public string ProductName { get; set; }

        public string ShortName { get; set; }       
        public string Type { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public string UpdateTime { get; set; }
        public string attributes { get; set; }
        public WareTree[] children { get; set; }
    }
}
