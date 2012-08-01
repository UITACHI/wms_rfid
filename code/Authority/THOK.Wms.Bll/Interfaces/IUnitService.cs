using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IUnitService:IService<Unit>
    {
        object GetDetails(int page, int rows, string UnitCode, string UnitName, string IsActive);

        bool Add(Unit unit);

        bool Delete(string UnitCode);

        bool Save(Unit unit);

        object FindUnit(string productCode);

    }
}
