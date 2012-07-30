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

        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            InBillAllotService.Allot(connectionId, data, new string[] { }, out data);
            return Connection.Send(connectionId, "success");
        }
    }
}