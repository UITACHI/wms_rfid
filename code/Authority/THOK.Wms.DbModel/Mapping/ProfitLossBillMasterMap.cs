using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class ProfitLossBillMasterMap : EntityMappingBase<ProfitLossBillMaster>
    {
        public ProfitLossBillMasterMap()
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
                .HasMaxLength(1);

            this.Property(t => t.CheckBillNo)
                .HasMaxLength(20);

            this.Property(t => t.WarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OperatePersonCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.VerifyPersonID)
                 .IsRequired();

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
            this.Property(t => t.CheckBillNo).HasColumnName(ColumnMap.Value.To("CheckBillNo"));
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.OperatePersonCode).HasColumnName(ColumnMap.Value.To("OperatePersonCode"));
            this.Property(t => t.Status).HasColumnName(ColumnMap.Value.To("Status"));
            this.Property(t => t.VerifyPersonID).HasColumnName(ColumnMap.Value.To("VerifyPersonCode"));
            this.Property(t => t.VerifyDate).HasColumnName(ColumnMap.Value.To("VerifyDate"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.BillType)
                .WithMany(t => t.ProfitLossBillMasters)
                .HasForeignKey(d => d.BillTypeCode)
                .WillCascadeOnDelete(false);
        }
    }
}
