using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class DeliverLine
    {
        public DeliverLine()
        {
            this.SortOrderDispatchs = new List<SortOrderDispatch>();
            this.SortOrders = new List<SortOrder>();
        }
        public string DeliverLineCode { get; set; }
        public string CustomCode { get; set; }
        public string DeliverLineName { get; set; }
        public string DistCode { get; set; }
        public int DeliverOrder { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<SortOrderDispatch> SortOrderDispatchs { get; set; }
        public virtual ICollection<SortOrder> SortOrders { get; set; }
    }
}
