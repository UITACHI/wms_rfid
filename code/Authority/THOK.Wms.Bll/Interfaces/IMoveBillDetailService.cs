using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IMoveBillDetailService :IService<MoveBillDetail>
    {
        object GetDetails(int page, int rows, string BillNo);

        bool Add(MoveBillDetail moveBillDetail);

        bool Delete(string ID);

        bool Save(MoveBillDetail moveBillDetail);
    }
}
