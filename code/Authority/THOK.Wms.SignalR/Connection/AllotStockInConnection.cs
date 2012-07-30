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
            // Broadcast data to all clients
            data = string.Format("数据是:{0} 时间是:{1}", data, DateTime.Now.ToString());
            Connection.Send(connectionId, data);
            Connection.Send(connectionId, data);
            Connection.Send(connectionId, data);
            InBillAllotService.Allot(connectionId, "ddd", new string[] { }, out data);
            return Connection.Send(connectionId, data);
        }
    }
}