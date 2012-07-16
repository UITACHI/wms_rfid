using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class CompanyMap : EntityMappingBase<Company>
    {
        public CompanyMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.CompanyCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CompanyType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UniformCode)
                .HasMaxLength(20);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("CompanyID"));
            this.Property(t => t.CompanyCode).HasColumnName(ColumnMap.Value.To("CompanyCode"));
            this.Property(t => t.CompanyName).HasColumnName(ColumnMap.Value.To("CompanyName"));
            this.Property(t => t.CompanyType).HasColumnName(ColumnMap.Value.To("CompanyType"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.ParentCompanyID).HasColumnName(ColumnMap.Value.To("ParentCompanyID"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.WarehouseSpace).HasColumnName(ColumnMap.Value.To("WarehouseSpace"));
            this.Property(t => t.WarehouseCount).HasColumnName(ColumnMap.Value.To("WarehouseCount"));
            this.Property(t => t.WarehouseCapacity).HasColumnName(ColumnMap.Value.To("WarehouseCapacity"));
            this.Property(t => t.SortingCount).HasColumnName(ColumnMap.Value.To("SortingCount"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.ParentCompany)
                .WithMany(t => t.Companies)
                .HasForeignKey(d => d.ParentCompanyID)
                .WillCascadeOnDelete(false);
        }
    }
}
