using System.Threading.Tasks;
using SignalR;
using System;
using THOK.Wms.SignalR.Allot.Interfaces;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Threading;
using THOK.Wms.SignalR.Model;
using System.Web;

namespace THOK.Wms.SignalR.Connection
{
    public class ConnectionBase : PersistentConnection
    {
        class ActionData
        {
            public string ActionType { get; set; }
            public object Data { get; set; }
        }

        private static IDictionary<string, CancellationTokenSource> dicCTS = new Dictionary<string, CancellationTokenSource>();
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
        protected JsonNetSerializer jns = new JsonNetSerializer();

        protected override Task OnReceivedAsync(IRequest request, string connectionId, string data)
        {
            string userName = HttpContext.Current.User != null ? HttpContext.Current.User.Identity.Name : string.Empty;
            ProgressState ps = new ProgressState();
            //try
            //{
                ActionData ad = jns.Parse<ActionData>(data);
                //start:开始分配；stop:停止分配
                switch (ad.ActionType)
                {
                    case "start":
                        Execute(connectionId,data,ps,GetCancellationTokenSource(connectionId).Token,userName);
                        break;
                    case "stop":
                        GetCancellationTokenSource(connectionId).Cancel();
                        ps.State = StateType.Stop;
                        return Connection.Send(connectionId, ps.Clone());
                    default:
                        break;
                }
            //}
            //catch (Exception e)
            //{
            //    ps.State = StateType.Error;
            //    ps.Messages.Add(e.Message);
            //}
            if (GetCancellationTokenSource(connectionId).Token.IsCancellationRequested)
            {
                ps.Messages.Clear();
                ps.Errors.Clear();
                ps.Messages.Add("用户已中止当前处理！");
            }
            ps.State = StateType.Complete;
            return Connection.Send(connectionId, ps.Clone());
        }

        protected virtual void Execute(string connectionId, string data, ProgressState ps, CancellationToken cancellationToken,string userName)
        {
            throw new NotImplementedException();
        }

        protected override Task OnDisconnectAsync(string connectionId)
        {
            GetCancellationTokenSource(connectionId).Cancel();
            return base.OnDisconnectAsync(connectionId);
        }
    }
}