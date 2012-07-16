using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class UserModuleMap : EntityMappingBase<UserModule>
    {
        public UserModuleMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.UserModuleID);

            // Properties
            // Table & Column Mappings
            this.Property(t => t.UserModuleID).HasColumnName(ColumnMap.Value.To("UserModuleID"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UserSystem_UserSystemID).HasColumnName(ColumnMap.Value.To("UserSystem_UserSystemID"));
            this.Property(t => t.Module_ModuleID).HasColumnName(ColumnMap.Value.To("Module_ModuleID"));

            // Relationships
            this.HasRequired(t => t.Module)
                .WithMany(t => t.UserModules)
                .HasForeignKey(d => d.Module_ModuleID);
            this.HasRequired(t => t.UserSystem)
                .WithMany(t => t.UserModules)
                .HasForeignKey(d => d.UserSystem_UserSystemID);

        }
    }
}
