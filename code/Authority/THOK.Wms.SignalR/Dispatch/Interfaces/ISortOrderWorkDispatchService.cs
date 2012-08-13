using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.SignalR.Model;
using System.Threading;

namespace THOK.Wms.SignalR.Dispatch.Interfaces
{
    public interface ISortOrderWorkDispatchService
    {
        void Dispatch(string connectionId, ProgressState ps, CancellationToken cancellationToken, string workDispatchId, string userName);
    }
}
