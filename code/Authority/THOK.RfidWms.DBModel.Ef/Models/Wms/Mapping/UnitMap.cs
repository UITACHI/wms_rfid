using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms.Mapping
{
    public class UnitMap : EntityMappingBase<Unit>
    {
        public UnitMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.UnitCode);

            // Table & Column Mappings
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.UnitName).HasColumnName(ColumnMap.Value.To("UnitName"));
            this.Property(t => t.COUNT).HasColumnName(ColumnMap.Value.To("COUNT"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));
           
            // Relationships
        }
    }
}
