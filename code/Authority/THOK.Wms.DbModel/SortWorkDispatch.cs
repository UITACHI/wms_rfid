using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Wms.DbModel
{
  public  class SortWorkDispatch
    {
      public SortWorkDispatch()
      {
          this.SortOrderDispatchs = new List<SortOrderDispatch>();
      }
        public Guid ID { get; set; }
        public string OrderDate { get; set; }
        public string SortingLineCode { get; set; }
        public string DispatchBatch { get; set; }
        public string OutBillNo { get; set; }
        public string MoveBillNo { get; set; }
        public string DispatchStatus { get; set; }
        public string IsActive { get; set; }
        public DateTime UpdateTime { get; set; }
        
        public virtual SortingLine SortingLine { get; set; }
        public virtual OutBillMaster OutBillMaster { get; set; }
        public virtual MoveBillMaster MoveBillMaster { get; set; }

        public virtual ICollection<SortOrderDispatch> SortOrderDispatchs { get; set; }
    }
}
