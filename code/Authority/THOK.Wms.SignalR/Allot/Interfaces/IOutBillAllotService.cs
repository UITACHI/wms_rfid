using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.SignalR.Allot.Interfaces;

namespace THOK.Wms.SignalR.Allot.Interfaces
{
    public interface IOutBillAllotService
    {
        bool Allot(string billNo, string[] areaCode);
    }
}
