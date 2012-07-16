using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class LoginLogMap : EntityMappingBase<LoginLog>
    {
        public LoginLogMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.LogID);

            // Properties
            this.Property(t => t.LoginPC)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.LoginTime)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.LogoutTime)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.Property(t => t.LogID).HasColumnName(ColumnMap.Value.To("LogID"));
            this.Property(t => t.LoginPC).HasColumnName(ColumnMap.Value.To("LoginPC"));
            this.Property(t => t.LoginTime).HasColumnName(ColumnMap.Value.To("LoginTime"));
            this.Property(t => t.LogoutTime).HasColumnName(ColumnMap.Value.To("LogoutTime"));
            this.Property(t => t.User_UserID).HasColumnName(ColumnMap.Value.To("User_UserID"));
            this.Property(t => t.System_SystemID).HasColumnName(ColumnMap.Value.To("System_SystemID"));

            // Relationships
            this.HasRequired(t => t.System)
                .WithMany(t => t.LoginLogs)
                .HasForeignKey(d => d.System_SystemID);
            this.HasRequired(t => t.User)
                .WithMany(t => t.LoginLogs)
                .HasForeignKey(d => d.User_UserID);

        }
    }
}
