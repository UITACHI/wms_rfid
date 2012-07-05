using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class CityMap : EntityMappingBase<City>
    {
        public CityMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.CityID);

            // Properties
            this.Property(t => t.CityName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.Property(t => t.CityID).HasColumnName(ColumnMap.Value.To("CityID"));
            this.Property(t => t.CityName).HasColumnName(ColumnMap.Value.To("CityName"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
        }
    }
}
