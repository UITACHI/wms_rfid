using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class ProductMap : EntityMappingBase<Product>
    {
        public ProductMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ProductCode);

            // Properties
            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UniformCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CustomCode)
                .HasMaxLength(20);

            this.Property(t => t.ShortCode)
                .HasMaxLength(10);

            this.Property(t => t.UnitListCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SupplierCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.BrandCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AbcTypeCode)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ProductTypeCode)
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.PackTypeCode)
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.PriceLevelCode)
                .HasMaxLength(4);

            this.Property(t => t.StatisticType)
                .HasMaxLength(10);

            this.Property(t => t.PieceBarcode)
                .HasMaxLength(13);

            this.Property(t => t.BarBarcode)
                .HasMaxLength(13);

            this.Property(t => t.PackageBarcode)
                .HasMaxLength(13);

            this.Property(t => t.OneProjectBarcode)
                .HasMaxLength(30);

            this.Property(t => t.BuyPrice)
                .HasPrecision(18, 2);

            this.Property(t => t.TradePrice)
                .HasPrecision(18, 2);

            this.Property(t => t.RetailPrice)
                .HasPrecision(18, 2);

            this.Property(t => t.CostPrice)
                .HasPrecision(18, 2);

            this.Property(t => t.IsFilterTip)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsNew)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsFamous)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsMainProduct)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsProvinceMainProduct)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.BelongRegion)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsConfiscate)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsAbnormity)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.ProductName).HasColumnName(ColumnMap.Value.To("ProductName"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.CustomCode).HasColumnName(ColumnMap.Value.To("CustomCode"));
            this.Property(t => t.ShortCode).HasColumnName(ColumnMap.Value.To("ShortCode"));
            this.Property(t => t.UnitListCode).HasColumnName(ColumnMap.Value.To("UnitListCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.SupplierCode).HasColumnName(ColumnMap.Value.To("SupplierCode"));
            this.Property(t => t.BrandCode).HasColumnName(ColumnMap.Value.To("BrandCode"));
            this.Property(t => t.AbcTypeCode).HasColumnName(ColumnMap.Value.To("AbcTypeCode"));
            this.Property(t => t.ProductTypeCode).HasColumnName(ColumnMap.Value.To("ProductTypeCode"));
            this.Property(t => t.PackTypeCode).HasColumnName(ColumnMap.Value.To("PackTypeCode"));
            this.Property(t => t.PriceLevelCode).HasColumnName(ColumnMap.Value.To("PriceLevelCode"));
            this.Property(t => t.StatisticType).HasColumnName(ColumnMap.Value.To("StatisticType"));
            this.Property(t => t.PieceBarcode).HasColumnName(ColumnMap.Value.To("PieceBarcode"));
            this.Property(t => t.BarBarcode).HasColumnName(ColumnMap.Value.To("BarBarcode"));
            this.Property(t => t.PackageBarcode).HasColumnName(ColumnMap.Value.To("PackageBarcode"));
            this.Property(t => t.OneProjectBarcode).HasColumnName(ColumnMap.Value.To("OneProjectBarcode"));
            this.Property(t => t.BuyPrice).HasColumnName(ColumnMap.Value.To("BuyPrice"));
            this.Property(t => t.TradePrice).HasColumnName(ColumnMap.Value.To("TradePrice"));
            this.Property(t => t.RetailPrice).HasColumnName(ColumnMap.Value.To("RetailPrice"));
            this.Property(t => t.CostPrice).HasColumnName(ColumnMap.Value.To("CostPrice"));
            this.Property(t => t.IsFilterTip).HasColumnName(ColumnMap.Value.To("IsFilterTip"));
            this.Property(t => t.IsNew).HasColumnName(ColumnMap.Value.To("IsNew"));
            this.Property(t => t.IsFamous).HasColumnName(ColumnMap.Value.To("IsFamous"));
            this.Property(t => t.IsMainProduct).HasColumnName(ColumnMap.Value.To("IsMainProduct"));
            this.Property(t => t.IsProvinceMainProduct).HasColumnName(ColumnMap.Value.To("IsProvinceMainProduct"));
            this.Property(t => t.BelongRegion).HasColumnName(ColumnMap.Value.To("BelongRegion"));
            this.Property(t => t.IsConfiscate).HasColumnName(ColumnMap.Value.To("IsConfiscate"));
            this.Property(t => t.IsAbnormity).HasColumnName(ColumnMap.Value.To("IsAbnormity"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.Brand)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.BrandCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Supplier)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.SupplierCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.UnitList)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.UnitListCode)
                .WillCascadeOnDelete(false);
           
        }
    }
}
