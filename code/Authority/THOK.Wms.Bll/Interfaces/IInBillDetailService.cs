using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IInBillDetailService:IService<InBillDetail>
    {
        object GetDetails(int page, int rows, string BillNo);

        bool Add(InBillDetail inBillDetail);

        bool Delete(string BillNo);

        bool Save(InBillDetail inBillDetail);
    }
}
