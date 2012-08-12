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
    public class AllotStockOutConnection : ConnectionBase
    {
        class ActionData
        {
            public string ActionType { get; set; }
            public string BillNo { get; set; }
            public string[] AreaCodes { get; set; }
        }

        [Dependency]
        public IOutBillAllotService OutBillAllotService { get; set; }

        protected override void Execute(string connectionId, string data, ProgressState ps, CancellationToken cancellationToken,string userName)
        {
            ActionData ad = jns.Parse<ActionData>(data);
            OutBillAllotService.Allot(connectionId, ps, cancellationToken, ad.BillNo, ad.AreaCodes);
        }                     
    }
}