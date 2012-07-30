using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class SortOrderDetailMap : EntityMappingBase<SortOrderDetail>
    {
        public SortOrderDetailMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.OrderDetailID);

            // Properties
            this.Property(t => t.OrderDetailID)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OrderID)
                .HasMaxLength(12);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProductName)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.UnitName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DemandQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.RealQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.UnitQuantity)
                .IsRequired()
                .HasPrecision(18, 2);


            // Table & Column Mappings
            this.Property(t => t.OrderDetailID).HasColumnName(ColumnMap.Value.To("OrderDetailID"));
            this.Property(t => t.OrderID).HasColumnName(ColumnMap.Value.To("OrderID"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.ProductName).HasColumnName(ColumnMap.Value.To("ProductName"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.UnitName).HasColumnName(ColumnMap.Value.To("UnitName"));
            this.Property(t => t.DemandQuantity).HasColumnName(ColumnMap.Value.To("DemandQuantity"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));
            this.Property(t => t.Price).HasColumnName(ColumnMap.Value.To("Price"));
            this.Property(t => t.Amount).HasColumnName(ColumnMap.Value.To("Amount"));
            this.Property(t => t.UnitQuantity).HasColumnName(ColumnMap.Value.To("UnitQuantity"));


            // Relationships
            this.HasRequired(t => t.SortOrder)
                .WithMany(t => t.SortOrderDetails)
                .HasForeignKey(d => d.OrderID)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.SortOrderDetails)
                .HasForeignKey(d => d.ProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.SortOrderDetails)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);
        }
    }
}
