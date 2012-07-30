using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class SortOrder
    {
        public SortOrder()
        {
            this.SortOrderDetails = new List<SortOrderDetail>();
        }
        public string OrderID { get; set; }
        public string CompanyCode { get; set; }
        public string SaleRegionCode { get; set; }
        public string OrderDate { get; set; }
        public string OrderType { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string DeliverLineCode { get; set; }
        public decimal QuantitySum { get; set; }
        public decimal AmountSum { get; set; }
        public decimal DetailNum { get; set; }
        public int DeliverOrder { get; set; }
        public string DeliverDate { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        //public virtual Company Company { get; set; }
        public virtual DeliverLine DeliverLine { get; set; }

        public virtual ICollection<SortOrderDetail> SortOrderDetails { get; set; }

    }
}