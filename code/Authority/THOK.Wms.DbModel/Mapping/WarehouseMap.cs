using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class WarehouseMap : EntityMappingBase<Warehouse>
    {
        public WarehouseMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.WarehouseCode);

            // Properties
            this.Property(t => t.WarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.WarehouseName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.WarehouseType)
                .HasMaxLength(1);

            this.Property(t => t.CompanyCode)
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .HasMaxLength(100);
           
            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.WarehouseName).HasColumnName(ColumnMap.Value.To("WarehouseName"));
            this.Property(t => t.ShortName).HasColumnName(ColumnMap.Value.To("ShortName"));
            this.Property(t => t.WarehouseType).HasColumnName(ColumnMap.Value.To("WarehouseType"));
            this.Property(t => t.CompanyCode).HasColumnName(ColumnMap.Value.To("CompanyCode"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
        }
    }
}
