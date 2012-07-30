using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.Wms.DbModel.Mapping
{
    public class InBillDetailMap : EntityMappingBase<InBillDetail>
    {
        public InBillDetailMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity); 

            this.Property(t => t.BillNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.BillQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.AllotQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.RealQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.BillNo).HasColumnName(ColumnMap.Value.To("BillNo"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.Price).HasColumnName(ColumnMap.Value.To("Price"));
            this.Property(t => t.BillQuantity).HasColumnName(ColumnMap.Value.To("BillQuantity"));
            this.Property(t => t.AllotQuantity).HasColumnName(ColumnMap.Value.To("AllotQuantity"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));

            // Relationships
            this.HasRequired(t => t.InBillMaster)
                .WithMany(t => t.InBillDetails)
                .HasForeignKey(d => d.BillNo)
                .WillCascadeOnDelete(false);
        }
    }
}
