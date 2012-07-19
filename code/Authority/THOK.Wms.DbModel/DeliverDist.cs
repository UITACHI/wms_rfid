using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class DeliverDist
    {
        public DeliverDist()
        {
            this.DeliverLines = new List<DeliverLine>();
        }
        public string DistCode { get; set; }
        public string CustomCode { get; set; }
        public string DistName { get; set; }
        public string DistCenterCode { get; set; }
        public string CompanyCode { get; set; }
        public string UniformCode { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual Company Company { get; set; }
        public virtual Company DistCenter { get; set; }
        public virtual ICollection<DeliverLine> DeliverLines { get; set; }

    }
}
