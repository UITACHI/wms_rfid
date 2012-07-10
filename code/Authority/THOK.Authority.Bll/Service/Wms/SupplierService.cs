using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Bll.Interfaces.Wms;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces.Wms;

namespace THOK.Authority.Bll.Service.Wms
{
    public class SupplierService:ServiceBase<Supplier>,ISupplierService
    {
        [Dependency]
        public ISupplierRepository SupplierRepository { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
