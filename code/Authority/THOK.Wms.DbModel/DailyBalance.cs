using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class DailyBalance
    {
        public DailyBalance()
        {            
        }

        public Guid ID { get; set; }
        public DateTime SettleDate { get; set; }
        public string WarehouseCode { get; set; }
        public string ProductCode { get; set; }
        public string UnitCode { get; set; }
        public decimal Beginning { get; set; }
        public decimal EntryAmount { get; set; }
        public decimal DeliveryAmount { get; set; }
        public decimal ProfitAmount { get; set; }
        public decimal LossAmount { get; set; }
        public decimal Ending { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }        
    }
}
