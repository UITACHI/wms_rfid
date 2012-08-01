using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
   public class SortOrderDispatch
   {
       public int ID { get; set; }
       public string OrderDate { get; set; }
       public string SortingLineCode { get; set; }
       public string DeliverLineCode { get; set; }
       public Guid? SortWorkDispatchID { get; set; }
       public string WorkStatus { get; set; }
       public string IsActive { get; set; }
       public DateTime UpdateTime { get; set; }

       public virtual SortingLine SortingLine { get; set; }
       public virtual DeliverLine DeliverLine { get; set; }
       public virtual SortWorkDispatch SortWorkDispatch { get; set; }
    }
}
