using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class Brand
    {
        public Brand()
        { 

        }
        public string BrandCode { get; set; }
        public string UniformCode { get; set; }
        public string CustomCode { get; set; }
        public string BrandName { get; set; }
        public string SupplierCode { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
