using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms.Mapping
{
    public class UnitListMap : EntityMappingBase<UnitList>
    {
        public UnitListMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.UnitListCode);

            // Table & Column Mappings
            this.Property(t => t.UnitListCode).HasColumnName(ColumnMap.Value.To("UnitListCode"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.UnitListName).HasColumnName(ColumnMap.Value.To("UnitListName"));
            this.Property(t => t.UnitCode01).HasColumnName(ColumnMap.Value.To("UnitCode01"));
            this.Property(t => t.UnitName01).HasColumnName(ColumnMap.Value.To("UnitName01"));
            this.Property(t => t.Quantity01).HasColumnName(ColumnMap.Value.To("Quantity01"));
            this.Property(t => t.UnitCode02).HasColumnName(ColumnMap.Value.To("UnitCode02"));
            this.Property(t => t.UnitName02).HasColumnName(ColumnMap.Value.To("UnitName02"));
            this.Property(t => t.Quantity02).HasColumnName(ColumnMap.Value.To("Quantity02"));
            this.Property(t => t.UnitCode03).HasColumnName(ColumnMap.Value.To("UnitCode03"));
            this.Property(t => t.UnitName03).HasColumnName(ColumnMap.Value.To("UnitName03"));
            this.Property(t => t.Quantity03).HasColumnName(ColumnMap.Value.To("Quantity03"));
            this.Property(t => t.UnitCode04).HasColumnName(ColumnMap.Value.To("UnitCode04"));
            this.Property(t => t.UnitName04).HasColumnName(ColumnMap.Value.To("UnitName04"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
        }
    }
}
