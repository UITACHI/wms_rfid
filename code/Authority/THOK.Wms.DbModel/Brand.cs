using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Brand
    {
        public Brand()
        {
            this.Products = new List<Product>();
        }
        public string BrandCode { get; set; }
        public string UniformCode { get; set; }
        public string CustomCode { get; set; }
        public string BrandName { get; set; }
        public string SupplierCode { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
