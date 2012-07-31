using System.Threading.Tasks;
using SignalR;
using System;
using THOK.Wms.SignalR.Allot.Interfaces;
using Microsoft.Practices.Unity;

namespace THOK.Wms.SignalR.Connection
{
    public class AllotStockInConnection : PersistentConnection
    {
        [Dependency]
        public IInBillAllotService InBillAllotService { get; set; }

        public Func<string, string, string[],string,bool> Fun = null;
        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            InBillAllotService.Allot(connectionId, data, new string[] { }, out data);
            Task.Factory.CancellationToken.ThrowIfCancellationRequested();
            return Connection.Send(connectionId, "success");
        }
    }
}