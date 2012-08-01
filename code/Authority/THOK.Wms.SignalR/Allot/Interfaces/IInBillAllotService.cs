using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using System.Threading;
using THOK.Wms.SignalR.Model;

namespace THOK.Wms.SignalR.Allot.Interfaces
{
    public interface IInBillAllotService
    {
        void Allot(string connectionId, ProgressState ps, CancellationToken cancellationToken, string billNo, string[] areaCodes);
    }
}
