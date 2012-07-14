using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Authority.DbModel.Mapping
{
    public class SystemEventLogMap : EntityMappingBase<SystemEventLog>
    {
        public SystemEventLogMap()
            : base("Auth")
        {
            // Primary Key
            this.HasKey(t => t.EventLogID);

            // Properties
            this.Property(t => t.EventLogTime)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.EventType)
                .IsRequired();

            this.Property(t => t.EventName)
                .IsRequired();

            this.Property(t => t.EventDescription)
                .IsRequired();

            this.Property(t => t.FromPC)
                .IsRequired();

            this.Property(t => t.OperateUser)
                .IsRequired();

            this.Property(t => t.TargetSystem)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.EventLogID).HasColumnName(ColumnMap.Value.To("EventLogID"));
            this.Property(t => t.EventLogTime).HasColumnName(ColumnMap.Value.To("EventLogTime"));
            this.Property(t => t.EventType).HasColumnName(ColumnMap.Value.To("EventType"));
            this.Property(t => t.EventName).HasColumnName(ColumnMap.Value.To("EventName"));
            this.Property(t => t.EventDescription).HasColumnName(ColumnMap.Value.To("EventDescription"));
            this.Property(t => t.FromPC).HasColumnName(ColumnMap.Value.To("FromPC"));
            this.Property(t => t.OperateUser).HasColumnName(ColumnMap.Value.To("OperateUser"));
            this.Property(t => t.TargetSystem).HasColumnName(ColumnMap.Value.To("TargetSystem"));
        }
    }
}
