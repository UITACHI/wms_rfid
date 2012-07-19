using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class DeliverDistMap : EntityMappingBase<DeliverDist>
    {
        public DeliverDistMap()
            : base("Wms") 
        {
            // Primary Key
            this.HasKey(t => t.DistCode);

            // Properties
            this.Property(t => t.DistCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CustomCode)
                .HasMaxLength(50);

            this.Property(t => t.DistName)
                .IsRequired()
                .HasMaxLength(100);

           this.Property(t => t.DistCenterCode)
                .HasMaxLength(20);

           this.Property(t => t.CompanyCode)
                .HasMaxLength(20);

            this.Property(t => t.UniformCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();


            // Table & Column Mappings
            this.Property(t => t.DistCode).HasColumnName(ColumnMap.Value.To("DistCode"));
            this.Property(t => t.CustomCode).HasColumnName(ColumnMap.Value.To("CustomCode"));
            this.Property(t => t.DistName).HasColumnName(ColumnMap.Value.To("DistName"));
            this.Property(t => t.DistCenterCode).HasColumnName(ColumnMap.Value.To("DistCenterCode"));
            this.Property(t => t.CompanyCode).HasColumnName(ColumnMap.Value.To("CompanyCode"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships

        
        }
    }
}
