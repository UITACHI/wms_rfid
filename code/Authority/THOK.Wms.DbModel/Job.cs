using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
    public class Job
    {
        public Job()
        {
            this.Employees = new List<Employee>();
        }

        public Guid ID { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public string Description { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
