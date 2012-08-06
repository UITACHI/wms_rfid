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

        bool Add(MoveBillDetail moveBillDetail, out string strResult);

        bool Delete(string ID, out string strResult);

        bool Save(MoveBillDetail moveBillDetail, out string strResult);
    }
}
