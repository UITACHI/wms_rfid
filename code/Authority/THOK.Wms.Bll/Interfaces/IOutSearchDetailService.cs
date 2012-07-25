using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IOutSearchDetailService : IService<OutBillAllot>
    {
        object GetDetails(int page, int rows, string BillNo);
    }
}
