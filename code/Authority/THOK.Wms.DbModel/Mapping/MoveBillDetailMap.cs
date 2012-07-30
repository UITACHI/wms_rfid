using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class MoveBillDetailMap : EntityMappingBase<MoveBillDetail>
    {
        public MoveBillDetailMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired();

            this.Property(t => t.BillNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OutCellCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OutStorageCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.InCellCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.InStorageCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UnitCode)
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
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.OutCellCode).HasColumnName(ColumnMap.Value.To("OutCellCode"));
            this.Property(t => t.OutStorageCode).HasColumnName(ColumnMap.Value.To("OutStorageCode"));
            this.Property(t => t.InCellCode).HasColumnName(ColumnMap.Value.To("InCellCode"));
            this.Property(t => t.InStorageCode).HasColumnName(ColumnMap.Value.To("InStorageCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.RealQuantity).HasColumnName(ColumnMap.Value.To("RealQuantity"));
            this.Property(t => t.OperatePersonID).HasColumnName(ColumnMap.Value.To("OperatePersonID"));
            this.Property(t => t.StartTime).HasColumnName(ColumnMap.Value.To("StartTime"));
            this.Property(t => t.FinishTime).HasColumnName(ColumnMap.Value.To("FinishTime"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));

            // Relationships
            this.HasRequired(t => t.MoveBillMaster)
                .WithMany(t => t.MoveBillDetails)
                .HasForeignKey(d => d.BillNo)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.MoveBillDetails)
                .HasForeignKey(d => d.ProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OutCell)
                .WithMany(t => t.MoveBillDetailOutCells)
                .HasForeignKey(d => d.OutCellCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OutStorage)
                .WithMany(t => t.MoveBillDetailOutStorages)
                .HasForeignKey(d => d.OutStorageCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.InCell)
                .WithMany(t => t.MoveBillDetailInCells)
                .HasForeignKey(d => d.InCellCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.InStorage)
                .WithMany(t => t.MoveBillDetailInStorages)
                .HasForeignKey(d => d.InStorageCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.MoveBillDetails)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.OperatePerson)
                .WithMany(t => t.MoveBillDetails)
                .HasForeignKey(d => d.OperatePersonID)
                .WillCascadeOnDelete(false);

        }
    }
}
