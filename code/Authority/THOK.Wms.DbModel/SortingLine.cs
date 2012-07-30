using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
   public class SortingLine
    {
       public SortingLine()
       {
           this.SortingLowerlimits = new List<SortingLowerlimit>();
           this.SortOrderDispatchs = new List<SortOrderDispatch>();
       }
       public string SortingLineCode { get; set; }
       public string SortingLineName { get; set; }
       public string SortingLineType { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }

       public virtual ICollection<SortingLowerlimit> SortingLowerlimits { get; set; }
       public virtual ICollection<SortOrderDispatch> SortOrderDispatchs { get; set; }
    }
}
