using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms.Mapping
{
    public class BrandMap : EntityMappingBase<Brand>
    {
        public BrandMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.BrandCode);

            // Table & Column Mappings
            this.Property(t => t.BrandCode).HasColumnName(ColumnMap.Value.To("BrandCode"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.CustomCode).HasColumnName(ColumnMap.Value.To("CustomCode"));
            this.Property(t => t.BrandName).HasColumnName(ColumnMap.Value.To("BrandName"));
            this.Property(t => t.SupplierCode).HasColumnName(ColumnMap.Value.To("SupplierCode"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
        }
    }
}
