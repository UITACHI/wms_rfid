using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class BillTypeMap : EntityMappingBase<BillType>
    {
        public BillTypeMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.BillTypeCode);

            // Properties
            this.Property(t => t.BillTypeCode)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.BillTypeName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.BillClass)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.BillTypeCode).HasColumnName(ColumnMap.Value.To("BillTypeCode"));
            this.Property(t => t.BillTypeName).HasColumnName(ColumnMap.Value.To("BillTypeName"));
            this.Property(t => t.BillClass).HasColumnName(ColumnMap.Value.To("BillClass"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
        }
    }
}
