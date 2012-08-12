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

        object GetMoveStorgeDetails(int page, int rows, string type, string id, string inOrOut, string productCode);

        object GetMoveInStorgeDetails(int page, int rows, string type, string id, string cellCode, string productCode);
    }
}
