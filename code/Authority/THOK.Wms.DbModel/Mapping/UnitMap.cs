using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class UnitMap : EntityMappingBase<Unit>
    {
        public UnitMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.UnitCode);

            // Properties
            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UnitName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            this.Property(t => t.RowVersion).IsRowVersion();

            // Table & Column Mappings
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.UnitName).HasColumnName(ColumnMap.Value.To("UnitName"));
            this.Property(t => t.Count).HasColumnName(ColumnMap.Value.To("COUNT"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));
            this.Property(t => t.RowVersion).HasColumnName(ColumnMap.Value.To("RowVersion"));
            // Relationships
        }
    }
}
