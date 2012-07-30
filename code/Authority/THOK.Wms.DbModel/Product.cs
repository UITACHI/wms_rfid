using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Product
    {
        public Product()
        {
            this.Cells = new List<Cell>();
            this.Storages = new List<Storage>();
            this.DailyBalances = new List<DailyBalance>();
            this.MoveBillDetails = new List<MoveBillDetail>();
            this.CheckBillDetailProducts = new List<CheckBillDetail>();
            this.CheckBillDetailRealProducts = new List<CheckBillDetail>();            
            this.ProfitLossBillDetails = new List<ProfitLossBillDetail>();
            this.SortOrderDetails = new List<SortOrderDetail>();
            this.InBillAllots = new List<InBillAllot>();
            this.OutBillAllots = new List<OutBillAllot>();
            this.SortingLowerlimits = new List<SortingLowerlimit>();
        }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UniformCode { get; set; }
        public string CustomCode { get; set; }
        public string ShortCode { get; set; }
        public string UnitListCode { get; set; }
        public string UnitCode { get; set; }
        public string SupplierCode { get; set; }
        public string BrandCode { get; set; }
        public string AbcTypeCode { get; set; }
        public string ProductTypeCode { get; set; }
        public string PackTypeCode { get; set; }
        public string PriceLevelCode { get; set; }
        public string StatisticType { get; set; }
        public string PieceBarcode { get; set; }
        public string BarBarcode { get; set; }
        public string PackageBarcode { get; set; }
        public string OneProjectBarcode { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal TradePrice { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal CostPrice { get; set; }
        public string IsFilterTip { get; set; }
        public string IsNew { get; set; }
        public string IsFamous { get; set; }
        public string IsMainProduct { get; set; }
        public string IsProvinceMainProduct { get; set; }
        public string BelongRegion { get; set; }
        public string IsConfiscate { get; set; }
        public string IsAbnormity { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual UnitList UnitList { get; set; }

        public virtual ICollection<Cell> Cells { get; set; }
        public virtual ICollection<Storage> Storages { get; set; }
        public virtual ICollection<DailyBalance> DailyBalances { get; set; }
        public virtual ICollection<MoveBillDetail> MoveBillDetails { get; set; }
        public virtual ICollection<CheckBillDetail> CheckBillDetailProducts { get; set; }
        public virtual ICollection<CheckBillDetail> CheckBillDetailRealProducts { get; set; }
        public virtual ICollection<ProfitLossBillDetail> ProfitLossBillDetails { get; set; }
        public virtual ICollection<SortOrderDetail> SortOrderDetails { get; set; }

        public virtual ICollection<InBillAllot> InBillAllots { get; set; }
        public virtual ICollection<OutBillAllot> OutBillAllots { get; set; }
        public virtual ICollection<SortingLowerlimit> SortingLowerlimits { get; set; }
    }
}
