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

            // Properties
            this.Property(t => t.BrandCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UniformCode)
                .HasMaxLength(20);

            this.Property(t => t.CustomCode)
                .HasMaxLength(20);

            this.Property(t => t.BrandName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SupplierCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

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
