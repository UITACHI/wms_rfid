using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class SortWorkDispatchService : ServiceBase<SortWorkDispatch>, ISortWorkDispatchService
    {
        protected override Type LogPrefix
        {
            get { throw new NotImplementedException(); }
        }
    }
}
