using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Service
{
    public class CheckBillDetailService : ServiceBase<CheckBillDetail>, ICheckBillDetailService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICheckBillDetailService 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            throw new NotImplementedException();
        }

        public new bool Add(CheckBillDetail inBillDetail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(CheckBillDetail inBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
