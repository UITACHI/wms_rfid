using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class StorageService : ServiceBase<Storage>, IStorageService
    {
        protected override Type LogPrefix
        {
            get { throw new NotImplementedException(); }
        }
    }
}
