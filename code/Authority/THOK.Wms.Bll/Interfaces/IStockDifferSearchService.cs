using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IStockDifferSearchService : IService<ProfitLossBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string CheckBillNo, string WarehouseCode, string BeginDate, string EndDate, string OperatePersonCode, string CheckPersonCode, string Operate_Status);
    }
}
