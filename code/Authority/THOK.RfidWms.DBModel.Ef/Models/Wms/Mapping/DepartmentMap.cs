using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms.Mapping
{
    public class DepartmentMap : EntityMappingBase<Department>
    {
        public DepartmentMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.DepartmentCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DepartmentName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.UniformCode)
                .HasMaxLength(20);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("DepartmentID"));
            this.Property(t => t.DepartmentCode).HasColumnName(ColumnMap.Value.To("DepartmentCode"));
            this.Property(t => t.DepartmentName).HasColumnName(ColumnMap.Value.To("DepartmentName"));
            this.Property(t => t.DepartmentLeaderID)
                .HasColumnName(ColumnMap.Value.To("DepartmentLeaderID"))
                .IsOptional();
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.CompanyID).HasColumnName(ColumnMap.Value.To("CompanyID"));
            this.Property(t => t.ParentDepartmentID).HasColumnName(ColumnMap.Value.To("ParentDepartmentID"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasOptional(t => t.DepartmentLeader)
                .WithMany(t => t.LeadDepartments)
                .HasForeignKey(d => d.DepartmentLeaderID)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Company)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.CompanyID)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.ParentDepartment)
                .WithMany(t => t.Departments)
                .HasForeignKey(d => d.ParentDepartmentID)
                .WillCascadeOnDelete(false);
        }
    }
}
