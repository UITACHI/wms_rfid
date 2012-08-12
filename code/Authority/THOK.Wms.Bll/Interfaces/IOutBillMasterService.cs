using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IOutBillMasterService:IService<OutBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(OutBillMaster outBillMaster, string userName, out string errorInfo);

        bool Delete(string BillNo, out string errorInfo);

        bool Save(OutBillMaster outBillMaster, out string errorInfo);

        object GenInBillNo(string userName);

        bool Audit(string billNo, string userName, out string errorInfo);

        bool AntiTrial(string billNo, out string errorInfo);

        bool Settle(string billNo,out string errorInfo);
    }
}
