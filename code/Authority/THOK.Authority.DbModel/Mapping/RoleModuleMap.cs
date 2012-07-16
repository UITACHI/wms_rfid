using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class RoleModuleMap : EntityMappingBase<RoleModule>
    {
        public RoleModuleMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.RoleModuleID);

            // Properties
            // Table & Column Mappings
            this.Property(t => t.RoleModuleID).HasColumnName(ColumnMap.Value.To("RoleModuleID"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.RoleSystem_RoleSystemID).HasColumnName(ColumnMap.Value.To("RoleSystem_RoleSystemID"));
            this.Property(t => t.Module_ModuleID).HasColumnName(ColumnMap.Value.To("Module_ModuleID"));

            // Relationships
            this.HasRequired(t => t.Module)
                .WithMany(t => t.RoleModules)
                .HasForeignKey(d => d.Module_ModuleID);
            this.HasRequired(t => t.RoleSystem)
                .WithMany(t => t.RoleModules)
                .HasForeignKey(d => d.RoleSystem_RoleSystemID);

        }
    }
}
