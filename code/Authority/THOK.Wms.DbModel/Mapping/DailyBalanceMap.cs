using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common.Ef.MappingStrategy;

namespace THOK.Wms.DbModel.Mapping
{
    public class DailyBalanceMap : EntityMappingBase<DailyBalance>
    {
        public DailyBalanceMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.SettleDate)
                .IsRequired();

            this.Property(t => t.WarehouseCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ProductCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.UnitCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Beginning)
                .IsRequired();

            this.Property(t => t.EntryAmount)
                .IsRequired();

            this.Property(t => t.DeliveryAmount)
                .IsRequired();

            this.Property(t => t.ProfitAmount)
                .IsRequired();

            this.Property(t => t.LossAmount)
                .IsRequired();

            this.Property(t => t.Ending)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.ID).HasColumnName(ColumnMap.Value.To("ID"));
            this.Property(t => t.SettleDate).HasColumnName(ColumnMap.Value.To("SettleDate"));
            this.Property(t => t.WarehouseCode).HasColumnName(ColumnMap.Value.To("WarehouseCode"));
            this.Property(t => t.ProductCode).HasColumnName(ColumnMap.Value.To("ProductCode"));
            this.Property(t => t.UnitCode).HasColumnName(ColumnMap.Value.To("UnitCode"));
            this.Property(t => t.Beginning).HasColumnName(ColumnMap.Value.To("Beginning"));
            this.Property(t => t.EntryAmount).HasColumnName(ColumnMap.Value.To("EntryAmount"));
            this.Property(t => t.DeliveryAmount).HasColumnName(ColumnMap.Value.To("DeliveryAmount"));
            this.Property(t => t.ProfitAmount).HasColumnName(ColumnMap.Value.To("ProfitAmount"));
            this.Property(t => t.LossAmount).HasColumnName(ColumnMap.Value.To("LossAmount"));
            this.Property(t => t.Ending).HasColumnName(ColumnMap.Value.To("Ending"));

            // Relationships
            this.HasRequired(t => t.Warehouse)
                .WithMany(t => t.DailyBalances)
                .HasForeignKey(d => d.WarehouseCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.DailyBalances)
                .HasForeignKey(d => d.ProductCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Unit)
                .WithMany(t => t.DailyBalances)
                .HasForeignKey(d => d.UnitCode)
                .WillCascadeOnDelete(false);
        }
    }
}
