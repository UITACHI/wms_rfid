using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class UserSystemMap : EntityMappingBase<UserSystem>
    {
        public UserSystemMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.UserSystemID);

            // Properties
            // Table & Column Mappings
            this.Property(t => t.UserSystemID).HasColumnName(ColumnMap.Value.To("UserSystemID"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.User_UserID).HasColumnName(ColumnMap.Value.To("User_UserID"));
            this.Property(t => t.City_CityID).HasColumnName(ColumnMap.Value.To("City_CityID"));
            this.Property(t => t.System_SystemID).HasColumnName(ColumnMap.Value.To("System_SystemID"));

            // Relationships
            this.HasRequired(t => t.City)
                .WithMany(t => t.UserSystems)
                .HasForeignKey(d => d.City_CityID);
            this.HasRequired(t => t.System)
                .WithMany(t => t.UserSystems)
                .HasForeignKey(d => d.System_SystemID);
            this.HasRequired(t => t.User)
                .WithMany(t => t.UserSystems)
                .HasForeignKey(d => d.User_UserID);

        }
    }
}
