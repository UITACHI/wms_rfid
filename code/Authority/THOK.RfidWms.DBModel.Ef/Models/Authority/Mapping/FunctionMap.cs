using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class FunctionMap : EntityMappingBase<Function>
    {
        public FunctionMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.FunctionID);

            // Properties
            this.Property(t => t.FunctionName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ControlName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.IndicateImage)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.Property(t => t.FunctionID).HasColumnName(ColumnMap.Value.To("FunctionID"));
            this.Property(t => t.FunctionName).HasColumnName(ColumnMap.Value.To("FunctionName"));
            this.Property(t => t.ControlName).HasColumnName(ColumnMap.Value.To("ControlName"));
            this.Property(t => t.IndicateImage).HasColumnName(ColumnMap.Value.To("IndicateImage"));
            this.Property(t => t.Module_ModuleID).HasColumnName(ColumnMap.Value.To("Module_ModuleID"));

            // Relationships
            this.HasRequired(t => t.Module)
                .WithMany(t => t.Functions)
                .HasForeignKey(d => d.Module_ModuleID);

        }
    }
}
