using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.SignalR.Allot.Interfaces
{
    public interface IInBillAllotService
    {
        bool Allot(string connectionId,string billNo,string [] areaCode,out string result);
    }
}
