using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IProfitLossBillMasterService:IService<ProfitLossBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string WareHouseCode, string BeginDate, string EndDate, string OperatePersonCode, string CheckPersonCode, string Status, string IsActive);

        bool Add(ProfitLossBillMaster profitLossBillMaster, string userName);

        bool Delete(string BillNo, out string strResult);

        bool Save(ProfitLossBillMaster profitLossBillMaster, out string strResult);

        object GenProfitLossBillNo(string userName);

        bool Audit(string BillNo, string userName, out string strResult);
    }
}
