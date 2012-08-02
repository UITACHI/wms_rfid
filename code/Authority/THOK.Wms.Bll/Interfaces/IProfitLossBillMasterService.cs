using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IProfitLossBillMasterService:IService<ProfitLossBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(ProfitLossBillMaster profitLossBillMaster, string userName);

        bool Delete(string BillNo);

        bool Save(ProfitLossBillMaster profitLossBillMaster);

        object GenProfitLossBillNo(string userName);

        bool Audit(string BillNo, string userName);
    }
}
