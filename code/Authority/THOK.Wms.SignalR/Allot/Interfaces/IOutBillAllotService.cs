using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.SignalR.Allot.Interfaces;
using THOK.Wms.SignalR.Model;
using System.Threading;

namespace THOK.Wms.SignalR.Allot.Interfaces
{
    public interface IOutBillAllotService
    {
        void Allot(string connectionId, ProgressState ps, CancellationToken cancellationToken, string billNo, string[] areaCodes);
    }
}
