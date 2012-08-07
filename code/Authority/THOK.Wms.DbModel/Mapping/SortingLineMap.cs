using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class SortingLineMap : EntityMappingBase<SortingLine>
    {
        public SortingLineMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.SortingLineCode);

            // Properties
            this.Property(t => t.SortingLineCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SortingLineName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.OutBillTypeCode)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.MoveBillTypeCode)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.CellCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.SortingLineType)
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
            this.Property(t => t.SortingLineCode).HasColumnName(ColumnMap.Value.To("SortingLineCode"));
            this.Property(t => t.SortingLineName).HasColumnName(ColumnMap.Value.To("SortingLineName"));
            this.Property(t => t.SortingLineType).HasColumnName(ColumnMap.Value.To("SortingLineType"));
            this.Property(t => t.OutBillTypeCode).HasColumnName(ColumnMap.Value.To("OutBillTypeCode"));
            this.Property(t => t.MoveBillTypeCode).HasColumnName(ColumnMap.Value.To("MoveBillTypeCode"));
            this.Property(t => t.CellCode).HasColumnName(ColumnMap.Value.To("CellCode"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasRequired(t => t.Cell)
                .WithMany()
                .HasForeignKey(d => d.CellCode)
                .WillCascadeOnDelete(false);
        }
    }
}
