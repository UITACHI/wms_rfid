using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace THOK.Authority.Bll.Interfaces.Wms
{
    public interface IShelfService : IService<Shelf>
    {
        object GetDetails(int page, int rows, string shelfCode);

        bool Add(Shelf shelf);

        bool Delete(string shelfCode);

        bool Save(Shelf shelf);
    }
}
