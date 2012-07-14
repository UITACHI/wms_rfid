using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IAreaService : IService<Area>
    {
        object GetDetails(int page, int rows,string areaCode);

        bool Add(Area area);

        bool Delete(string areaCode);

        bool Save(Area area);
    }
}
