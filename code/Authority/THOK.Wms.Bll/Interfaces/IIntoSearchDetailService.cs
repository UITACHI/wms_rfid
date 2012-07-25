using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IIntoSearchDetailService : IService<InBillAllot>
    {
        object GetDetails(int page, int rows, string BillNo);
    }
}
