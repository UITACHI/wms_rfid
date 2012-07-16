using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class JobMap : EntityMappingBase<Job>
    {
        public JobMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.JobCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.JobName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("JobID"));
            this.Property(t => t.JobCode).HasColumnName(ColumnMap.Value.To("JobCode"));
            this.Property(t => t.JobName).HasColumnName(ColumnMap.Value.To("JobName"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));
        }
    }
}
