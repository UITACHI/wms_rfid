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

        object GetDetails(int page, int rows, string ware, string area, string shelf, string cell);
    }
}
