using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using THOK.Common.Ef.MappingStrategy;
using THOK.Wms.DbModel;

namespace THOK.Wms.DbModel.Mapping
{
    public class CustomerMap : EntityMappingBase<Customer>
    {
            public CustomerMap()
            : base("Wms")
        {
            // Primary Key
            this.HasKey(t => t.CustomerCode);
            // Properties
            this.Property(t => t.CustomerCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CustomCode)
                .HasMaxLength(50);

            this.Property(t => t.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CompanyCode)
                 .HasMaxLength(20);

            this.Property(t => t.SaleRegionCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.UniformCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CustomerType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(4);

            this.Property(t => t.SaleScope)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.IndustryType)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CityOrCountryside)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.DeliverLineCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.DeliverOrder)
                .IsRequired();

            this.Property(t => t.Address)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Phone)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.LicenseType)
                .HasMaxLength(10);

            this.Property(t => t.LicenseCode)
                .HasMaxLength(20);

            this.Property(t => t.PrincipalName)
                 .HasMaxLength(20);

            this.Property(t => t.PrincipalPhone)
                .HasMaxLength(60);

            this.Property(t => t.PrincipalAddress)
                .HasMaxLength(100);

            this.Property(t => t.ManagementName)
                .HasMaxLength(20);

            this.Property(t => t.ManagementPhone)
                .HasMaxLength(60);

            this.Property(t => t.Bank)
                .HasMaxLength(50);

            this.Property(t => t.BankAccounts)
                .HasMaxLength(50);


            this.Property(t => t.Description)
                .HasMaxLength(100);

            this.Property(t => t.IsActive)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.UpdateTime)
                .IsRequired();

            // Table & Column Mappings
            this.Property(t => t.CustomerCode).HasColumnName(ColumnMap.Value.To("CustomerCode"));
            this.Property(t => t.CustomCode).HasColumnName(ColumnMap.Value.To("CustomCode"));
            this.Property(t => t.CustomerName).HasColumnName(ColumnMap.Value.To("CustomerName"));
            this.Property(t => t.CompanyCode).HasColumnName(ColumnMap.Value.To("CompanyCode"));
            this.Property(t => t.SaleRegionCode).HasColumnName(ColumnMap.Value.To("SaleRegionCode"));
            this.Property(t => t.UniformCode).HasColumnName(ColumnMap.Value.To("UniformCode"));
            this.Property(t => t.CustomerType).HasColumnName(ColumnMap.Value.To("CustomerType"));
            this.Property(t => t.SaleScope).HasColumnName(ColumnMap.Value.To("SaleScope"));
            this.Property(t => t.IndustryType).HasColumnName(ColumnMap.Value.To("IndustryType"));
            this.Property(t => t.CityOrCountryside).HasColumnName(ColumnMap.Value.To("CityOrCountryside"));
            this.Property(t => t.DeliverLineCode).HasColumnName(ColumnMap.Value.To("DeliverLineCode"));
            this.Property(t => t.DeliverOrder).HasColumnName(ColumnMap.Value.To("DeliverOrder"));
            this.Property(t => t.Address).HasColumnName(ColumnMap.Value.To("Address"));
            this.Property(t => t.Phone).HasColumnName(ColumnMap.Value.To("Phone"));
            this.Property(t => t.LicenseType).HasColumnName(ColumnMap.Value.To("LicenseType"));
            this.Property(t => t.LicenseCode).HasColumnName(ColumnMap.Value.To("LicenseCode"));
            this.Property(t => t.PrincipalName).HasColumnName(ColumnMap.Value.To("PrincipalName"));
            this.Property(t => t.PrincipalPhone).HasColumnName(ColumnMap.Value.To("PrincipalPhone"));
            this.Property(t => t.PrincipalAddress).HasColumnName(ColumnMap.Value.To("PrincipalAddress"));
            this.Property(t => t.ManagementName).HasColumnName(ColumnMap.Value.To("ManagementName"));
            this.Property(t => t.ManagementPhone).HasColumnName(ColumnMap.Value.To("ManagementPhone"));
            this.Property(t => t.Bank).HasColumnName(ColumnMap.Value.To("Bank"));
            this.Property(t => t.BankAccounts).HasColumnName(ColumnMap.Value.To("BankAccounts"));
            this.Property(t => t.Description).HasColumnName(ColumnMap.Value.To("Description"));
            this.Property(t => t.IsActive).HasColumnName(ColumnMap.Value.To("IsActive"));
            this.Property(t => t.UpdateTime).HasColumnName(ColumnMap.Value.To("UpdateTime"));

            // Relationships
            this.HasOptional(t => t.Company)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.CompanyCode)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.DeliverLine)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.DeliverLineCode)
                .WillCascadeOnDelete(false);
        }
    }
}
