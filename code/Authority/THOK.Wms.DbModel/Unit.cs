﻿using System;
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
            this.DailyBalances = new List<DailyBalance>();
            this.MoveBillDetails = new List<MoveBillDetail>();
            this.CheckBillDetails = new List<CheckBillDetail>();
        }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public int COUNT { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<DailyBalance> DailyBalances { get; set; }
        public virtual ICollection<MoveBillDetail> MoveBillDetails { get; set; }
        public virtual ICollection<CheckBillDetail> CheckBillDetails { get; set; }
    }
}
