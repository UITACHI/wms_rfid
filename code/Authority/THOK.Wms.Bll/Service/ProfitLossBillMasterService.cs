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
    public class ProfitLossBillMasterService:ServiceBase<ProfitLossBillMaster>,IProfitLossBillMasterService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IProfitLossBillMasterService 成员

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive)
        {
            throw new NotImplementedException();
        }

        public bool Add(ProfitLossBillMaster profitLossBillMaster, string userName)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(ProfitLossBillMaster profitLossBillMaster)
        {
            throw new NotImplementedException();
        }

        public object GenProfitLossBillNo(string userName)
        {
            throw new NotImplementedException();
        }

        public bool Audit(string BillNo, string userName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
