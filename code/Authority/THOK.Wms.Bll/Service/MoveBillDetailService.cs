using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class MoveBillDetailService:ServiceBase<MoveBillDetail>, IMoveBillDetailService
    {
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IMoveBillDetail 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            throw new NotImplementedException();
        }

        public new bool Add(MoveBillDetail moveBillDetail)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string ID)
        {
            throw new NotImplementedException();
        }

        public bool Save(MoveBillDetail moveBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
