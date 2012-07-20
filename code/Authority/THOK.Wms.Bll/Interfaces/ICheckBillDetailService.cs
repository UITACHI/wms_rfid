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

        bool CellAdd(string BillNo, string ware, string area, string shelf, string cell);

        bool Delete(string BillNo);

        bool Save(CheckBillDetail inBillDetail);
    }
}
