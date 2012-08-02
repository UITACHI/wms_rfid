using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IStockledgerService
    {
        object GetDetails(int page, int rows, string warehouseCode, string productCode, string beginDate, string endDate);
        object GetInfoDetails(int page, int rows, string warehouseCode, string productCode, string settleDate);
    }
}
