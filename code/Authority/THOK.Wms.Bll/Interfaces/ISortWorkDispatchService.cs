using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISortWorkDispatchService : IService<SortWorkDispatch>
    {
        object GetDetails(int page, int rows, string OrderDate, string SortingLineCode, string DispatchStatus);

        bool Add(SortWorkDispatch sortWorkDisp);

        bool Delete(string id);

        bool Save(SortWorkDispatch sortWorkDisp);

        bool Audit(string id, string userName);

        bool AntiTrial(string id);

        bool Settle(string id, out string errorInfo);
    }
}
