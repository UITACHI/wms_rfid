using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Warehouse
    {
        public Warehouse()
        {
            this.Areas = new List<Area>();
            this.Shelfs = new List<Shelf>();
            this.Cells = new List<Cell>();
            this.DailyBalances = new List<DailyBalance>();
            this.MoveBillMasters = new List<MoveBillMaster>();
            this.CheckBillMasters = new List<CheckBillMaster>();
            this.OutBillMasters = new List<OutBillMaster>();
            this.InBillMasters = new List<InBillMaster>();
            this.ProfitLossBillMasters = new List<ProfitLossBillMaster>();
        }

        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string ShortName { get; set; }
        public string WarehouseType { get; set; }
        public string CompanyCode { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Area> Areas { get; set; }
        public virtual ICollection<Shelf> Shelfs { get; set; }
        public virtual ICollection<Cell> Cells { get; set; }
        public virtual ICollection<DailyBalance> DailyBalances { get; set; }
        public virtual ICollection<MoveBillMaster> MoveBillMasters { get; set; }
        public virtual ICollection<CheckBillMaster> CheckBillMasters { get; set; }
        public virtual ICollection<OutBillMaster> OutBillMasters { get; set; }
        public virtual ICollection<InBillMaster> InBillMasters { get; set; }
        public virtual ICollection<ProfitLossBillMaster> ProfitLossBillMasters { get; set; }
    }
}
