using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class RoleMap : EntityMappingBase<Role>
    {
        public RoleMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.RoleID);

            // Properties
            this.Property(t => t.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Memo)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.Property(t => t.RoleID).HasColumnName(ColumnMap.Value.To("RoleID"));
            this.Property(t => t.RoleName).HasColumnName(ColumnMap.Value.To("RoleName"));
            this.Property(t => t.IsLock).HasColumnName(ColumnMap.Value.To("IsLock"));
            this.Property(t => t.Memo).HasColumnName(ColumnMap.Value.To("Memo"));
        }
    }
}
