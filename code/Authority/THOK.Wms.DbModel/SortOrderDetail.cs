using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class SortOrderDetail
    {
        public SortOrderDetail()
        {

        }
        public string OrderDetailID { get; set; }
        public string OrderID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public decimal DemandQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal UnitQuantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual SortOrder SortOrder { get; set; }
        public virtual Unit Unit { get; set; }
        
    }
}
