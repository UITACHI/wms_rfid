using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.RfidWms.DBModel.Ef.Models.Wms
{
    public class Unit
    {
        public Unit()
        { 

        }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public int COUNT { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
