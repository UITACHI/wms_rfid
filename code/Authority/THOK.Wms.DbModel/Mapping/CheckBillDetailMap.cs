using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.Wms.DbModel.Mapping
{
    public class CheckBillDetailMap : EntityMappingBase<CheckBillDetail>
    {
        public CheckBillDetailMap()
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

            this.Property(t => t.Quantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.RealProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.RealUnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.RealQuantity)
                .IsRequired()
                .HasPrecision(18, 2);

            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);


            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.BillNo).HasColumnName(ColumnMap.Value.To("BillNo"));
            this.Property(t => t.CellCode).HasColumnName(ColumnMap.Value.To("CellCode"));
            this.Property(t => t.StorageCode).HasColumnName(ColumnMap.Value.To("StorageCode"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.Quantity).HasColumnName(ColumnMap.Value.To("Quantity"));
            this.Property(t => t.RealProductCode).HasColumnName(ColumnMap.Value.To("RealProductCode"));
            this.Property(t => t.RealUnitCode).HasColumnName(ColumnMap.Value.To("RealUnitCode"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));
            this.Property(t => t.OperatePersonID).HasColumnName(ColumnMap.Value.To("OperatePersonID"));
            this.Property(t => t.StartTime).HasColumnName(ColumnMap.Value.To("StartTime"));
            this.Property(t => t.FinishTime).HasColumnName(ColumnMap.Value.To("FinishTime"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
            this.HasRequired(t => t.CheckBillMaster)
                .WithMany(t => t.CheckBillDetails)
                .HasForeignKey(d => d.BillNo)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Cell)
                .WithMany(t => t.CheckBillDetails)
                .HasForeignKey(d => d.CellCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.CheckBillDetailProducts)
                .HasForeignKey(d => d.ProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.RealProduct)
                .WithMany(t => t.CheckBillDetailRealProducts)
                .HasForeignKey(d => d.RealProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.CheckBillDetailUnits)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.RealUnit)
                .WithMany(t => t.CheckBillDetailRealUnits)
                .HasForeignKey(d => d.RealUnitCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Storage)
                .WithMany(t => t.CheckBillDetails)
                .HasForeignKey(d => d.StorageCode)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.OperatePerson)
                .WithMany(t => t.CheckBillDetails)
                .HasForeignKey(d => d.OperatePersonID)
                .WillCascadeOnDelete(false);
        }
    }
}
