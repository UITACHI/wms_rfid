using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.Wms.DbModel.Mapping
{
    public class ProfitLossBillDetailMap : EntityMappingBase<ProfitLossBillDetail>
    {
        public ProfitLossBillDetailMap()
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

            this.Property(t => t.CellCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.StorageCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.Quantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.BillNo).HasColumnName(ColumnMap.Value.To("BillNo"));
            this.Property(t => t.CellCode).HasColumnName(ColumnMap.Value.To("CellCode"));
            this.Property(t => t.StorageCode).HasColumnName(ColumnMap.Value.To("StorageCode"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.Price).HasColumnName(ColumnMap.Value.To("Price"));
            this.Property(t => t.Quantity).HasColumnName(ColumnMap.Value.To("Quantity"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));

            // Relationships
            this.HasRequired(t => t.ProfitLossBillMaster)
                .WithMany(t => t.ProfitLossBillDetails)
                .HasForeignKey(d => d.BillNo)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.ProfitLossBillDetails)
                .HasForeignKey(d => d.ProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.ProfitLossBillDetails)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Storage)
                .WithMany(t => t.ProfitLossBillDetails)
                .HasForeignKey(d => d.StorageCode)
                .WillCascadeOnDelete(false);
        }
    }
}
