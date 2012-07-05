using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.RfidWms.DBModel.Ef.Models.MappingStrategy;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping
{
    public class UserMap : EntityMappingBase<User>
    {
        public UserMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.UserID);

            // Properties
            this.Property(t => t.UserName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Pwd)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ChineseName)
                .HasMaxLength(50);

            this.Property(t => t.LoginPC)
                .HasMaxLength(50);

            this.Property(t => t.Memo)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.Property(t => t.UserID).HasColumnName(ColumnMap.Value.To("UserID"));
            this.Property(t => t.UserName).HasColumnName(ColumnMap.Value.To("UserName"));
            this.Property(t => t.Pwd).HasColumnName(ColumnMap.Value.To("Pwd"));
            this.Property(t => t.ChineseName).HasColumnName(ColumnMap.Value.To("ChineseName"));
            this.Property(t => t.IsLock).HasColumnName(ColumnMap.Value.To("IsLock"));
            this.Property(t => t.IsAdmin).HasColumnName(ColumnMap.Value.To("IsAdmin"));
            this.Property(t => t.LoginPC).HasColumnName(ColumnMap.Value.To("LoginPC"));
            this.Property(t => t.Memo).HasColumnName(ColumnMap.Value.To("Memo"));
        }
    }
}
