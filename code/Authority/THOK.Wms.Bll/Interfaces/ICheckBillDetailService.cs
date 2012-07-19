using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICheckBillDetailService : IService<CheckBillDetail>
    {
        object GetDetails(int page, int rows, string BillNo);

        bool Add(CheckBillDetail inBillDetail);

        bool Delete(string BillNo);

        bool Save(CheckBillDetail inBillDetail);
    }
}
