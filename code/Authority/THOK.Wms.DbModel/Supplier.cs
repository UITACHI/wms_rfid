using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Supplier
    {
        public Supplier()
        {
            this.Products = new List<Product>();
            this.Brands=new List<Brand>();
        }
        public string SupplierCode { get; set; }
        public string UniformCode { get; set; }
        public string CustomCode { get; set; }
        public string SupplierName { get; set; }
        public string ProvinceName { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
