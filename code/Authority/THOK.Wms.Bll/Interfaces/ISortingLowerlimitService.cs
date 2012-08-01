using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISortingLowerlimitService : IService<SortingLowerlimit>
    {
        object GetDetails(int page, int rows, string sortingLineCode, string sortingLineName, string productName, string productCode, string IsActive);

        bool Add(SortingLowerlimit sortLowerLimit);

        bool Delete(string id);

        bool Save(SortingLowerlimit sortLowerLimit);
    }
}
