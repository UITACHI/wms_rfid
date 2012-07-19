using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class ShelfMap : EntityMappingBase<Shelf>
    {
        public ShelfMap()
            : base("Wms")
        {

            // Primary Key
            this.HasKey(t => t.ShelfCode);

            // Properties
            this.Property(t => t.ShelfCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ShelfName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ShelfType)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.WarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AreaCode)
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
            this.Property(t => t.ShelfCode).HasColumnName(ColumnMap.Value.To("ShelfCode"));
            this.Property(t => t.ShelfName).HasColumnName(ColumnMap.Value.To("ShelfName"));
            this.Property(t => t.ShortName).HasColumnName(ColumnMap.Value.To("ShortName"));
            this.Property(t => t.ShelfType).HasColumnName(ColumnMap.Value.To("ShelfType"));
            this.Property(t => t.CellCols).HasColumnName(ColumnMap.Value.To("CellCols"));
            this.Property(t => t.CellRows).HasColumnName(ColumnMap.Value.To("CellRows"));
            this.Property(t => t.ImgX).HasColumnName(ColumnMap.Value.To("ImgX"));
            this.Property(t => t.ImgY).HasColumnName(ColumnMap.Value.To("ImgY"));
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.AreaCode).HasColumnName(ColumnMap.Value.To("AreaCode"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.Warehouse)
                .WithMany(t => t.Shelfs)
                .HasForeignKey(d => d.WarehouseCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Area)
                .WithMany(t => t.Shelfs)
                .HasForeignKey(d => d.AreaCode)
                .WillCascadeOnDelete(false);
        }
    }
}
