using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Customer
    {
        public string CustomerCode { get; set; }
        public string CustomCode { get; set; }
        public string CustomerName { get; set; }
        public string CompanyCode { get; set; }
        public string SaleRegionCode { get; set; }
        public string UniformCode { get; set; }
        public string CustomerType { get; set; }
        public string SaleScope { get; set; }
        public string IndustryType { get; set; }
        public string CityOrCountryside { get; set; }
        public string DeliverLineCode { get; set; }
        public int DeliverOrder { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string LicenseType { get; set; }
        public string LicenseCode { get; set; }
        public string PrincipalName { get; set; }
        public string PrincipalPhone { get; set; }
        public string PrincipalAddress { get; set; }
        public string ManagementName { get; set; }
        public string ManagementPhone { get; set; }
        public string Bank { get; set; }
        public string BankAccounts { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }


        public virtual Company Company { get; set; }
        public virtual DeliverLine DeliverLine { get; set; }
    }
}
