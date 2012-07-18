using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class AreaMap : EntityMappingBase<Area>
    {
        public AreaMap()
            : base("Wms")
        {

            // Primary Key
            this.HasKey(t => t.AreaCode);

            // Properties
            this.Property(t => t.AreaCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AreaName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.AreaType)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.AllotInOrder)
                .IsRequired();

            this.Property(t => t.AllotOutOrder)
                .IsRequired();

            this.Property(t => t.WarehouseCode)
                .IsRequired()
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
            this.Property(t => t.AreaCode).HasColumnName(ColumnMap.Value.To("AreaCode"));
            this.Property(t => t.AreaName).HasColumnName(ColumnMap.Value.To("AreaName"));
            this.Property(t => t.ShortName).HasColumnName(ColumnMap.Value.To("ShortName"));
            this.Property(t => t.AreaType).HasColumnName(ColumnMap.Value.To("AreaType"));
            this.Property(t => t.AllotInOrder).HasColumnName(ColumnMap.Value.To("AllotInOrder"));
            this.Property(t => t.AllotOutOrder).HasColumnName(ColumnMap.Value.To("AllotOutOrder"));
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.Warehouse)
                .WithMany(t => t.Areas)
                .HasForeignKey(d => d.WarehouseCode)
                .WillCascadeOnDelete(false);
        }
    }
}
