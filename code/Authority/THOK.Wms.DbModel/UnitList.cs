using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class UnitList
    {
        public UnitList()
        {
            this.Products = new List<Product>();
        }
        public string UnitListCode { get; set; }
        public string UniformCode { get; set; }
        public string UnitListName { get; set; }
        public string UnitCode01 { get; set; }
        public string UnitName01 { get; set; }
        public decimal Quantity01 { get; set; }
        public string UnitCode02 { get; set; }
        public string UnitName02 { get; set; }
        public decimal Quantity02 { get; set; }
        public string UnitCode03 { get; set; }
        public string UnitName03 { get; set; }
        public decimal Quantity03 { get; set; }
        public string UnitCode04 { get; set; }
        public string UnitName04 { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
