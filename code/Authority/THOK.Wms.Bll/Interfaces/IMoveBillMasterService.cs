using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IMoveBillMasterService : IService<MoveBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo,string WareHouseCode, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(MoveBillMaster moveBillMaster, string userName);

        bool Delete(string BillNo, out string strResult);

        bool Save(MoveBillMaster moveBillMaster, out string strResult);

        object GenMoveBillNo(string userName);

        bool Audit(string BillNo, string userName, out string strResult);

        bool AntiTrial(string BillNo, out string strResult);

        object GetBillTypeDetail(string BillClass, string IsActive);

        object GetWareHouseDetail(string IsActive);

        bool Settle(string BillNo, out string strResult);
    }
}
