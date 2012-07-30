using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class DeliverLineMap : EntityMappingBase<DeliverLine>
    {
          public DeliverLineMap()
            : base("Wms")
              {
                    // Primary Key
                  this.HasKey(t => t.DeliverLineCode);

                  // Properties
                  this.Property(t => t.DeliverLineCode)
                      .IsRequired()
                      .HasMaxLength(50);

                  this.Property(t => t.CustomCode)
                      .HasMaxLength(50);

                  this.Property(t => t.DeliverLineName)
                      .IsRequired()
                      .HasMaxLength(100);

                  this.Property(t => t.DistCode)
                      .HasMaxLength(50);

                  this.Property(t => t.DeliverOrder)
                      .IsRequired();

                  this.Property(t => t.Description)
                      .HasMaxLength(100);

                  this.Property(t => t.IsActive)
                      .IsRequired()
                      .IsFixedLength()
                      .HasMaxLength(1);

                  this.Property(t => t.UpdateTime)
                      .IsRequired();
                  // Table & Column Mappings

                  this.Property(t => t.DeliverLineCode).HasColumnName(ColumnMap.Value.To("DeliverLineCode"));
                  this.Property(t => t.CustomCode).HasColumnName(ColumnMap.Value.To("CustomCode"));
                  this.Property(t => t.DeliverLineName).HasColumnName(ColumnMap.Value.To("DeliverLineName"));
                  this.Property(t => t.DistCode).HasColumnName(ColumnMap.Value.To("DistCode"));
                  this.Property(t => t.DeliverOrder).HasColumnName(ColumnMap.Value.To("DeliverOrder"));
                  this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
                  this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
                  this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

                  // Relationships

              }


    }
}
