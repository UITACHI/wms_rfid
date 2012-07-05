using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class RoleFunctionMap : EntityMappingBase<RoleFunction>
    {
        public RoleFunctionMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.RoleFunctionID);

            // Properties
            // Table & Column Mappings
            this.Property(t => t.RoleFunctionID).HasColumnName(ColumnMap.Value.To("RoleFunctionID"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.RoleModule_RoleModuleID).HasColumnName(ColumnMap.Value.To("RoleModule_RoleModuleID"));
            this.Property(t => t.Function_FunctionID).HasColumnName(ColumnMap.Value.To("Function_FunctionID"));

            // Relationships
            this.HasRequired(t => t.Function)
                .WithMany(t => t.RoleFunctions)
                .HasForeignKey(d => d.Function_FunctionID);
            this.HasRequired(t => t.RoleModule)
                .WithMany(t => t.RoleFunctions)
                .HasForeignKey(d => d.RoleModule_RoleModuleID);

        }
    }
}
