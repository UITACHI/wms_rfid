using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class RoleSystemMap : EntityMappingBase<RoleSystem>
    {
        public RoleSystemMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.RoleSystemID);

            // Properties
            // Table & Column Mappings
            this.Property(t => t.RoleSystemID).HasColumnName(ColumnMap.Value.To("RoleSystemID"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.Role_RoleID).HasColumnName(ColumnMap.Value.To("Role_RoleID"));
            this.Property(t => t.City_CityID).HasColumnName(ColumnMap.Value.To("City_CityID"));
            this.Property(t => t.System_SystemID).HasColumnName(ColumnMap.Value.To("System_SystemID"));

            // Relationships
            this.HasRequired(t => t.City)
                .WithMany(t => t.RoleSystems)
                .HasForeignKey(d => d.City_CityID);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.RoleSystems)
                .HasForeignKey(d => d.Role_RoleID);
            this.HasRequired(t => t.System)
                .WithMany(t => t.RoleSystems)
                .HasForeignKey(d => d.System_SystemID);

        }
    }
}
