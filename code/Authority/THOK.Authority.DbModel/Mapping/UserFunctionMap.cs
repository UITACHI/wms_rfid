using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class UserFunctionMap : EntityMappingBase<UserFunction>
    {
        public UserFunctionMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.UserFunctionID);

            // Properties
            // Table & Column Mappings
            this.Property(t => t.UserFunctionID).HasColumnName(ColumnMap.Value.To("UserFunctionID"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UserModule_UserModuleID).HasColumnName(ColumnMap.Value.To("UserModule_UserModuleID"));
            this.Property(t => t.Function_FunctionID).HasColumnName(ColumnMap.Value.To("Function_FunctionID"));

            // Relationships
            this.HasRequired(t => t.Function)
                .WithMany(t => t.UserFunctions)
                .HasForeignKey(d => d.Function_FunctionID);
            this.HasRequired(t => t.UserModule)
                .WithMany(t => t.UserFunctions)
                .HasForeignKey(d => d.UserModule_UserModuleID);

        }
    }
}
