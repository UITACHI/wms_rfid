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

        bool Delete(string id,ref string errorInfo);

        bool Audit(string id, string userName,ref string errorInfo);

        bool AntiTrial(string id, ref string errorInfo);

        bool Settle(string id, ref string errorInfo);
    }
}
