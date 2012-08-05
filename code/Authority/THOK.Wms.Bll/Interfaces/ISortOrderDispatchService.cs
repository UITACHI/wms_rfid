using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISortOrderDispatchService : IService<SortOrderDispatch>
    {
        object GetDetails(int page, int rows, string OrderDate, string SortingLineCode);

        object GetWorkStatus(string WorkStatus);

        bool Add(string SortingLineCode, string DeliverLineCodes);

        bool Delete(string id);

        bool Save(SortOrderDispatch sortDispatch);

    }
}
