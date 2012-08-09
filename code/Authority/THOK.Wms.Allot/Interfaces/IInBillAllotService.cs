using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Allot.Interfaces
{
    public interface IInBillAllotService:IService<InBillAllot>
    {
        object Search(string billNo, int page, int rows);

        bool AllotConfirm(string billNo, out string strResult);

        bool AllotCancelConfirm(string billNo, out string strResult);

        bool AllotDelete(string billNo, long id, out string strResult);

        bool AllotEdit(string billNo, long id, string cellCode, int allotQuantity, out string strResult);

        bool AllotCancel(string billNo, out string strResult);

        bool AllotAdd(string billNo, long id, string cellCode, int allotQuantity, out string strResult);
    }
}
