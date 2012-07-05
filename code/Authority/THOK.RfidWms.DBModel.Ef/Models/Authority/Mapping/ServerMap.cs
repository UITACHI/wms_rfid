using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class ServerMap : EntityMappingBase<Server>
    {
        public ServerMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.ServerID);

            // Properties
            this.Property(t => t.ServerName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.Property(t => t.ServerID).HasColumnName(ColumnMap.Value.To("ServerID"));
            this.Property(t => t.ServerName).HasColumnName(ColumnMap.Value.To("ServerName"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.Url).HasColumnName(ColumnMap.Value.To("Url"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.City_CityID).HasColumnName(ColumnMap.Value.To("City_CityID"));

            // Relationships
            this.HasRequired(t => t.City)
                .WithMany(t => t.Servers)
                .HasForeignKey(d => d.City_CityID);

        }
    }
}
