using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IUnitListService:IService<UnitList>
    {
        object GetDetails(int page, int rows, string unitListCode);

        bool Add(UnitList unitList);

        bool Delete(string unitListCode);

        bool Save(UnitList unitList);
    }
}
