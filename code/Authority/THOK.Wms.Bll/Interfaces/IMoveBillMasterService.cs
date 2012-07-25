using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IMoveBillMasterService : IService<MoveBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(MoveBillMaster moveBillMaster, string userName);

        bool Delete(string BillNo);

        bool Save(MoveBillMaster moveBillMaster);

        object GenMoveBillNo(string userName);

        bool Audit(string BillNo, string userName);

        bool AntiTrial(string BillNo);

        object GetBillTypeDetail(string BillClass, string IsActive);

        object GetWareHouseDetail(string IsActive);
    }
}
