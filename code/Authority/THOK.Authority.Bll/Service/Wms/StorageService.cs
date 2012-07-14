using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Bll.Interfaces.Wms;

namespace THOK.Authority.Bll.Service.Wms
{
    public class StorageService : ServiceBase<Storage>, IStorageService
    {
        protected override Type LogPrefix
        {
            get { throw new NotImplementedException(); }
        }
    }
}
