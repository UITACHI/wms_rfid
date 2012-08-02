using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICargospaceService : IService<Storage>
    {
        object GetCellDetails(int page, int rows, string type, string id);
    }
}
