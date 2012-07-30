using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class SortOrderMap : EntityMappingBase<SortOrder>
    {
        public SortOrderMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.OrderID);

            // Properties
            this.Property(t => t.OrderID)
                .IsRequired()
                .HasMaxLength(12);

            this.Property(t => t.CompanyCode)
                .HasMaxLength(20);
            this.Property(t => t.DeliverLineCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SaleRegionCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OrderDate)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(14);

            this.Property(t => t.OrderType)
                .IsRequired()
                .HasMaxLength(1);

            this.Property(t => t.CustomerCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.QuantitySum)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.AmountSum)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.DetailNum)
                .IsRequired();

            this.Property(t => t.DeliverDate)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(14);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();


            // Table & Column Mappings
            this.Property(t => t.OrderID).HasColumnName(ColumnMap.Value.To("OrderID"));
            this.Property(t => t.CompanyCode).HasColumnName(ColumnMap.Value.To("CompanyCode"));
            this.Property(t => t.SaleRegionCode).HasColumnName(ColumnMap.Value.To("SaleRegionCode"));
            this.Property(t => t.OrderDate).HasColumnName(ColumnMap.Value.To("OrderDate"));
            this.Property(t => t.OrderType).HasColumnName(ColumnMap.Value.To("OrderType"));
            this.Property(t => t.CustomerCode).HasColumnName(ColumnMap.Value.To("CustomerCode"));
            this.Property(t => t.CustomerName).HasColumnName(ColumnMap.Value.To("CustomerName"));
            this.Property(t => t.DeliverLineCode).HasColumnName(ColumnMap.Value.To("DeliverLineCode"));
            this.Property(t => t.QuantitySum).HasColumnName(ColumnMap.Value.To("QuantitySum"));
            this.Property(t => t.AmountSum).HasColumnName(ColumnMap.Value.To("AmountSum"));
            this.Property(t => t.DetailNum).HasColumnName(ColumnMap.Value.To("DetailNum"));
            this.Property(t => t.DeliverOrder).HasColumnName(ColumnMap.Value.To("DeliverOrder"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));


            // Relationships
            this.HasRequired(t => t.DeliverLine)
                .WithMany(t => t.SortOrders)
                .HasForeignKey(d => d.DeliverLineCode)
                .WillCascadeOnDelete(false);

            //this.HasRequired(t => t.Company)
            //    .WithMany(t => t.SortOrders)
            //    .HasForeignKey(d => d.CompanyCode)
            //    .WillCascadeOnDelete(false);

        }
    }
}
