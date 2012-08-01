using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface IProfitLossBillDetailService:IService<ProfitLossBillDetail>
    {
        object GetDetails(int page, int rows, string BillNo);

        bool Add(ProfitLossBillDetail profitLossBillDetail);

        bool Delete(string ID);

        bool Save(ProfitLossBillDetail profitLossBillDetail);
    }
}
