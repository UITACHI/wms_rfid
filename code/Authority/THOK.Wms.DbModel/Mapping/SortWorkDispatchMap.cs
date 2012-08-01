using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class SortWorkDispatchMap : EntityMappingBase<SortWorkDispatch>
    {
       public SortWorkDispatchMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.OrderDate)
                .IsRequired()
                .HasMaxLength(14);

            this.Property(t => t.SortingLineCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DispatchBatch)
                .IsRequired()
                .HasMaxLength(2);

            this.Property(t => t.OutBillNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.MoveBillNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.DispatchStatus)
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
            this.Property(t => t.DispatchBatch).HasColumnName(ColumnMap.Value.To("DispatchBatch"));
            this.Property(t => t.OutBillNo).HasColumnName(ColumnMap.Value.To("OutBillNo"));
            this.Property(t => t.MoveBillNo).HasColumnName(ColumnMap.Value.To("MoveBillNo"));
            this.Property(t => t.DispatchStatus).HasColumnName(ColumnMap.Value.To("DispatchStatus")); 
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.MoveBillMaster)
                .WithMany()
                .HasForeignKey(d => d.MoveBillNo)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.OutBillMaster)
                .WithMany()
                .HasForeignKey(d => d.OutBillNo)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.SortingLine)
                .WithMany(t => t.SortWorkDispatchs)
                .HasForeignKey(d => d.SortingLineCode)
                .WillCascadeOnDelete(false);
        }
    }
}
