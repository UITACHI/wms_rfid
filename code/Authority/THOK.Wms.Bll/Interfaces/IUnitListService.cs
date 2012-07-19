using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IUnitListService:IService<UnitList>
    {
        object GetDetails(int page, int rows, UnitList ul);

        bool Add(UnitList unitList);

        bool Delete(string unitListCode);

        bool Save(UnitList unitList);
    }
}
