using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Allot.Interfaces;
using THOK.Wms.SignalR.Connection;

namespace THOK.Wms.SignalR.Allot.Service
{
    public class OutBillAllotService : Notifier<AllotStockInConnection>, IOutBillAllotService
    {
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }
        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }

        public void Allot(string connectionId, Model.ProgressState ps, System.Threading.CancellationToken cancellationToken, string billNo, string[] areaCodes)
        {
            throw new System.NotImplementedException();
        }
    }
}
