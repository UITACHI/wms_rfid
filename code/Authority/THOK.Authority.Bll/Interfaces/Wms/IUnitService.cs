using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IUnitService:IService<Unit>
    {
        object GetDetails(int page, int rows, string UnitCode, string UnitName, string IsActive);

        bool Add(Unit unit);

        bool Delete(string UnitCode);

        bool Save(Unit unit);
    }
}
