using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IStorageService : IService<Storage>
    {
        object GetDetails(int page, int rows, string type, string id);

        object GetCellDetails(int page, int rows, string ware, string area, string shelf, string cell);

        object GetProductDetails(int page, int rows, string products);

        object GetChangedCellDetails(int page, int rows, string beginDate, string endDate);
    }
}
