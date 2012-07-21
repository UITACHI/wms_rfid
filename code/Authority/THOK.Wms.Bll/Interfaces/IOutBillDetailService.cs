using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IOutBillDetailService:IService<OutBillDetail>
    {
        object GetDetails(int page, int rows, string BillNo);

        bool Add(OutBillDetail outBillDetail);

        bool Delete(string ID);

        bool Save(OutBillDetail outBillDetail);
    }
}
