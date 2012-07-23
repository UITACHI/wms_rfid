using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICheckBillMasterService : IService<CheckBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(string billNo,string wareCode);

        bool CellAdd(string ware, string area, string shelf, string cell,string UserName);

        bool Delete(string BillNo);

        bool Save(CheckBillMaster inBillMaster);

        object GetCheckBillNo();

    }
}
