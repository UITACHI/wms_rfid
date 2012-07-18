using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Unit
    {
        public Unit()
        {
            this.Products = new List<Product>();
            this.ProfitLossBillDetails = new List<ProfitLossBillDetail>();
            this.SortOrderDetails = new List<SortOrderDetail>();
        }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public int COUNT { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProfitLossBillDetail> ProfitLossBillDetails { get; set; }
        public virtual ICollection<SortOrderDetail> SortOrderDetails { get; set; }
    }
}
