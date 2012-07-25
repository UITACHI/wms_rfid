using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IMoveSearchDetailService : IService<MoveBillDetail>
    {
        object GetDetails(int page, int rows, string BillNo);
    }
}
