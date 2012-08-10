using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IStockCheckSearchService : IService<CheckBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string WarehouseCode, string BeginDate, string EndDate, string OperatePersonCode, string CheckPersonCode, string Operate_Status);
    }
}
