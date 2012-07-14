using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface ICellService : IService<Cell>
    {
        object GetDetails(int page, int rows, string cellCode);

        bool Add(Cell cell);

        bool Delete(string cellCode);

        bool Save(Cell cell);

        object GetSearch(string shelfCode);
    }
}
