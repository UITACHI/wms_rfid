using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Company
    {
        public Company()
        {
            this.Companies = new List<Company>();
            this.Departments = new List<Department>();
        }

        public Guid ID { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string Description { get; set; }
        public Guid ParentCompanyID { get; set; }
        public string UniformCode { get; set; }

        public decimal WarehouseSpace { get; set; }
        public decimal WarehouseCount { get; set; }
        public decimal WarehouseCapacity { get; set; }
        public decimal SortingCount { get; set; }

        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Company ParentCompany { get; set; }

        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
    }
}
