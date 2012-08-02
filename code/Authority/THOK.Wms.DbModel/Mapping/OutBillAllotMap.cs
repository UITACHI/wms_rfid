using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.Wms.DbModel.Mapping
{
    public class OutBillAllotMap : EntityMappingBase<OutBillAllot>
    {
        public OutBillAllotMap()
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

            this.Property(t => t.CellCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.StorageCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AllotQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.RealQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.OperatePersonID);

            this.Property(t => t.Status)
                 .IsRequired()
                 .IsFixedLength()
                 .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.BillNo).HasColumnName(ColumnMap.Value.To("BillNo"));
            this.Property(t => t.OutPalletTag).HasColumnName(ColumnMap.Value.To("OutPalletTag)"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.OutBillDetailId).HasColumnName(ColumnMap.Value.To("OutBillDetailId"));
            this.Property(t => t.CellCode).HasColumnName(ColumnMap.Value.To("CellCode"));
            this.Property(t => t.StorageCode).HasColumnName(ColumnMap.Value.To("StorageCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode")); 
            this.Property(t => t.AllotQuantity).HasColumnName(ColumnMap.Value.To("AllotQuantity"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));
            this.Property(t => t.OperatePersonID).HasColumnName(ColumnMap.Value.To("OperatePersonID"));
            this.Property(t => t.StartTime).HasColumnName(ColumnMap.Value.To("StartTime"));
            this.Property(t => t.FinishTime).HasColumnName(ColumnMap.Value.To("FinishTime"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
            this.HasRequired(t => t.OutBillMaster)
                .WithMany(t => t.OutBillAllots)
                .HasForeignKey(d => d.BillNo)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OutBillDetail)
                .WithMany()
                .HasForeignKey(d => d.OutBillDetailId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.OutBillAllots)
                .HasForeignKey(d => d.ProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Storage)
                .WithMany(t => t.OutBillAllots)
                .HasForeignKey(d => d.StorageCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.OutBillAllots)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Cell)
                .WithMany(t => t.OutBillAllots)
                .HasForeignKey(d => d.CellCode)
                .WillCascadeOnDelete(false);
        }

    }
}
