using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class ModuleMap : EntityMappingBase<Module>
    {
        public ModuleMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.ModuleID);

            // Properties
            this.Property(t => t.ModuleName)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ModuleURL)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.IndicateImage)
                .HasMaxLength(100);

            this.Property(t => t.DeskTopImage)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.Property(t => t.ModuleID).HasColumnName(ColumnMap.Value.To("ModuleID"));
            this.Property(t => t.ModuleName).HasColumnName(ColumnMap.Value.To("ModuleName"));
            this.Property(t => t.ShowOrder).HasColumnName(ColumnMap.Value.To("ShowOrder"));
            this.Property(t => t.ModuleURL).HasColumnName(ColumnMap.Value.To("ModuleURL"));
            this.Property(t => t.IndicateImage).HasColumnName(ColumnMap.Value.To("IndicateImage"));
            this.Property(t => t.DeskTopImage).HasColumnName(ColumnMap.Value.To("DeskTopImage"));
            this.Property(t => t.System_SystemID).HasColumnName(ColumnMap.Value.To("System_SystemID"));
            this.Property(t => t.ParentModule_ModuleID).HasColumnName(ColumnMap.Value.To("ParentModule_ModuleID"));

            // Relationships
            this.HasRequired(t => t.Module2)
                .WithMany(t => t.Module1)
                .HasForeignKey(d => d.ParentModule_ModuleID);
            this.HasRequired(t => t.System)
                .WithMany(t => t.Modules)
                .HasForeignKey(d => d.System_SystemID);

        }
    }
}
