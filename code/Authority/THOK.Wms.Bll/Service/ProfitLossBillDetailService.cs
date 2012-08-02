using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;

namespace THOK.Wms.Bll.Service
{
    public class ProfitLossBillDetailService:ServiceBase<ProfitLossBillDetail>,IProfitLossBillDetailService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IProfitLossBillDetailService 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            throw new NotImplementedException();
        }

        public new bool Add(ProfitLossBillDetail profitLossBillDetail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Save(ProfitLossBillDetail profitLossBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
