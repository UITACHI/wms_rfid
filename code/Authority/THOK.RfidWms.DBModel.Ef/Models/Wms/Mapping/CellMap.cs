using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms.Mapping
{
    public class CellMap : EntityMappingBase<Cell>
    {
        public CellMap()
            : base("Wms")
        {

            // Primary Key
            this.HasKey(t => t.CellCode);

            // Properties
            this.Property(t => t.CellCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CellName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CellType)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.Layer)
                .IsRequired();

            this.Property(t => t.Rfid)
                .HasMaxLength(100);

            this.Property(t => t.WarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AreaCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ShelfCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DefaultProductCode)
                .HasMaxLength(20);

            this.Property(t => t.MaxQuantity)
                .IsRequired();

            this.Property(t => t.IsSingle)
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
            this.Property(t => t.CellCode).HasColumnName(ColumnMap.Value.To("CellCode"));
            this.Property(t => t.CellName).HasColumnName(ColumnMap.Value.To("CellName"));
            this.Property(t => t.ShortName).HasColumnName(ColumnMap.Value.To("ShortName"));
            this.Property(t => t.CellType).HasColumnName(ColumnMap.Value.To("CellType"));
            this.Property(t => t.Layer).HasColumnName(ColumnMap.Value.To("Layer"));
            this.Property(t => t.Rfid).HasColumnName(ColumnMap.Value.To("Rfid"));
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.AreaCode).HasColumnName(ColumnMap.Value.To("AreaCode"));
            this.Property(t => t.ShelfCode).HasColumnName(ColumnMap.Value.To("ShelfCode"));
            this.Property(t => t.DefaultProductCode).HasColumnName(ColumnMap.Value.To("DefaultProductCode"));
            this.Property(t => t.MaxQuantity).HasColumnName(ColumnMap.Value.To("MaxQuantity"));
            this.Property(t => t.IsSingle).HasColumnName(ColumnMap.Value.To("IsSingle"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.warehouse)
                .WithMany(t => t.Cells)
                .HasForeignKey(d => d.WarehouseCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.area)
                .WithMany(t => t.Cells)
                .HasForeignKey(d => d.AreaCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.shelf)
                .WithMany(t => t.Cells)
                .HasForeignKey(d => d.ShelfCode)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.product)
                .WithMany(t => t.Cells)
                .HasForeignKey(d => d.DefaultProductCode)
                .WillCascadeOnDelete(false);
        }
    }
}
