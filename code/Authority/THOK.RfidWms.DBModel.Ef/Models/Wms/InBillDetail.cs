using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class InBillDetail
    {
        public InBillDetail()
        {
        }
        public int ID { get; set; }
        public string BillNo { get; set; }
        public string ProductCode { get; set; }
        public string UnitCode { get; set; }
        public decimal Price { get; set; }
        public decimal BillQuantity { get; set; }
        public decimal AllotQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public string Description { get; set; }
    }
}
