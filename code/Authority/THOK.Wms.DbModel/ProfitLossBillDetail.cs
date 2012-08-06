using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class ProfitLossBillDetail
    {
        public ProfitLossBillDetail()
        {
        }
        public int ID { get; set; }
        public string BillNo { get; set; }
        public string CellCode { get; set; }
        public string StorageCode { get; set; }
        public string ProductCode { get; set; }
        public string UnitCode { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }

        public virtual ProfitLossBillMaster ProfitLossBillMaster { get; set; }
        public virtual Product Product { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual Storage Storage { get; set; }

    }
}
