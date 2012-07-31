using System.Threading.Tasks;
using SignalR;
using System;
using THOK.Wms.SignalR.Allot.Interfaces;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Threading;
using THOK.Wms.SignalR.Model;

namespace THOK.Wms.SignalR.Connection
{
    public class AllotStockInConnection : PersistentConnection
    {
        class ActionData
        {
            public string ActionType { get; set; }
            public string BillNo { get; set; }
            public string[] AreaCodes { get; set; }
        }

        [Dependency]
        public IInBillAllotService InBillAllotService { get; set; }

        private IDictionary<string, CancellationTokenSource> dicCTS = new Dictionary<string, CancellationTokenSource>();

        private CancellationTokenSource GetCancellationTokenSource(string connectionId)
        {
            if (!dicCTS.ContainsKey(connectionId))
            {
                lock (dicCTS)
                {
                    if (!dicCTS.ContainsKey(connectionId))
                    {
                        dicCTS.Add(connectionId, new CancellationTokenSource());
                    }
                }
            }
            return dicCTS[connectionId];
        }

        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            ProgressState ps = new ProgressState();
            JsonNetSerializer jns = new JsonNetSerializer();

            //try
            //{
                ActionData ad = jns.Parse<ActionData>(data);
                //start:开始分配；stop:停止分配
                switch (ad.ActionType)
                {
                    case "start":
                        InBillAllotService.Allot(connectionId, ps, GetCancellationTokenSource(connectionId).Token, ad.BillNo, ad.AreaCodes);
                        break;
                    case "stop":
                        GetCancellationTokenSource(connectionId).Cancel();
                        break;
                    default:
                        break;
                }
            //}
            //catch (Exception e)
            //{
            //    ps.State = StateType.Error;
            //    ps.Messages.Add(e.Message);
            //}

            ps.State = StateType.Complete;
            return Connection.Send(connectionId, ps);
        }

        protected override Task OnDisconnectAsync(string connectionId)
        {
            GetCancellationTokenSource(connectionId).Cancel();
            return base.OnDisconnectAsync(connectionId);
        }
    }
}