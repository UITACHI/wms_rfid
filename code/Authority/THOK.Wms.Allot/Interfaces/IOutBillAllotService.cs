using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Allot.Interfaces;

namespace THOK.Wms.Allot.Interfaces
{
    public interface IOutBillAllotService:IService<OutBillAllot>
    {
        bool Allot(string billNo, string[] areaCode);
    }
}
