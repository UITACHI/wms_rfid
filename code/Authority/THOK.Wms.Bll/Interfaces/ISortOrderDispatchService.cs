using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISortOrderDispatchService : IService<SortOrderDispatch>
    {
       object GetDetails(int page, int rows, string OrderDate, string SortingLineCode, string DeliverLineCode);

        bool Add(SortOrderDispatch sortDispatch);

        bool Delete(string id);

        bool Save(SortOrderDispatch sortDispatch);
    }
}
