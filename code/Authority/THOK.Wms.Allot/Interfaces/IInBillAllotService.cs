using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Allot.Interfaces
{
    public interface IInBillAllotService:IService<InBillAllot>
    {
        bool Allot(string billNo,string [] areaCode,out string result);
    }
}
