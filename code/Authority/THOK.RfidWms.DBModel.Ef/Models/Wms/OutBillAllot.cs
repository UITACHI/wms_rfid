using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class OutBillAllot
    {
        public OutBillAllot()
        {
        }
        public int ID { get; set; }
        public string BillNo { get; set; }
        public int OutPalletTag { get; set; }
        public string ProductCode { get; set; }
        public string CellCode { get; set; }
        public string StorageCode { get; set; }
        public decimal AllotQuantity { get; set; }
        public decimal RealQuantity { get; set; }
        public string OperatePersonCode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public string Status { get; set; }
    }
}
