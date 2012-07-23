using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class CheckBillMasterMap : EntityMappingBase<CheckBillMaster>
    {
        public CheckBillMasterMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.BillNo);

            // Properties
            this.Property(t => t.BillNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.BillDate)
                .IsRequired();

            this.Property(t => t.BillTypeCode)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.WarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OperatePersonID)
                .IsRequired();

            this.Property(t => t.Status)
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
            this.Property(t => t.BillNo).HasColumnName(ColumnMap.Value.To("BillNo"));
            this.Property(t => t.BillDate).HasColumnName(ColumnMap.Value.To("BillDate"));
            this.Property(t => t.BillTypeCode).HasColumnName(ColumnMap.Value.To("BillTypeCode"));
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.OperatePersonID).HasColumnName(ColumnMap.Value.To("OperatePersonID"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));
            this.Property(t => t.VerifyPersonID).HasColumnName(ColumnMap.Value.To("VerifyPersonID"));
            this.Property(t => t.VerifyDate).HasColumnName(ColumnMap.Value.To("VerifyDate"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.BillType)
                .WithMany(t => t.CheckBillMasters)
                .HasForeignKey(d => d.BillTypeCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Warehouse)
                .WithMany(t => t.CheckBillMasters)
                .HasForeignKey(d => d.WarehouseCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OperatePerson)
                .WithMany(t => t.OperatePersonCheckBillMasters)
                .HasForeignKey(d => d.OperatePersonID)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.VerifyPerson)
                .WithMany(t => t.VerifyPersonCheckBillMasters)
                .HasForeignKey(d => d.VerifyPersonID)
                .WillCascadeOnDelete(false);
        }
    }
}
