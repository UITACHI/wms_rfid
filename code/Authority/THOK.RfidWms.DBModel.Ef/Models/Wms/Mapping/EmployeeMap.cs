using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms.Mapping
{
    public class EmployeeMap : EntityMappingBase<Employee>
    {
        public EmployeeMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.EmployeeCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.EmployeeName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Sex)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.Tel)
                .HasMaxLength(50);

            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("EmployeeID"));
            this.Property(t => t.EmployeeCode).HasColumnName(ColumnMap.Value.To("EmployeeCode"));
            this.Property(t => t.EmployeeName).HasColumnName(ColumnMap.Value.To("EmployeeName"));
            
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.DepartmentID).HasColumnName(ColumnMap.Value.To("DepartmentID"));
            this.Property(t => t.JobID).HasColumnName(ColumnMap.Value.To("JobID"));
            this.Property(t => t.Sex).HasColumnName(ColumnMap.Value.To("Sex"));
            this.Property(t => t.Tel).HasColumnName(ColumnMap.Value.To("Tel"));
            
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.Department)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.DepartmentID)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Job)
                .WithMany(t => t.Employees)
                .HasForeignKey(d => d.JobID)
                .WillCascadeOnDelete(false);
        }
    }
}
