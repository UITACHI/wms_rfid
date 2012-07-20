using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IOutBillMasterService:IService<OutBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(OutBillMaster outBillMaster);

        bool Delete(string BillNo);

        bool Save(OutBillMaster outBillMaster);

        object GenInBillNo();
    }
}
