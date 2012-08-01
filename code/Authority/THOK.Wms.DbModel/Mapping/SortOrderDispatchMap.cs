using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;
using System.ComponentModel.DataAnnotations.Schema;

namespace THOK.Wms.DbModel.Mapping
{
    public class SortOrderDispatchMap : EntityMappingBase<SortOrderDispatch>
    {
        public SortOrderDispatchMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.ID)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.OrderDate)
                .IsRequired()
                .HasMaxLength(14);

            this.Property(t => t.SortingLineCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DeliverLineCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.WorkStatus)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.OrderDate).HasColumnName(ColumnMap.Value.To("OrderDate"));
            this.Property(t => t.SortingLineCode).HasColumnName(ColumnMap.Value.To("SortingLineCode"));
            this.Property(t => t.DeliverLineCode).HasColumnName(ColumnMap.Value.To("DeliverLineCode"));
            this.Property(t => t.SortWorkDispatchID).HasColumnName(ColumnMap.Value.To("SortWorkDispatchID"));
            this.Property(t => t.WorkStatus).HasColumnName(ColumnMap.Value.To("WorkStatus"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.DeliverLine)
                .WithMany(t => t.SortOrderDispatchs)
                .HasForeignKey(d => d.DeliverLineCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.SortingLine)
                .WithMany(t => t.SortOrderDispatchs)
                .HasForeignKey(d => d.SortingLineCode)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.SortWorkDispatch)
                .WithMany(t => t.SortOrderDispatchs)
                .HasForeignKey(d => d.SortWorkDispatchID)
                .WillCascadeOnDelete(false);
        }
    }
}
