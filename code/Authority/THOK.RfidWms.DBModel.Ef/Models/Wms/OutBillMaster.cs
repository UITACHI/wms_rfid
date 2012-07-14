using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class OutBillMaster
    {
        public OutBillMaster()
        {
        }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string BillType { get; set; }
        public string WarehouseCode { get; set; }
        public string OperatePersonCode { get; set; }
        public string Status { get; set; }
        public string VerifyPersonCode { get; set; }
        public DateTime VerifyDate { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
