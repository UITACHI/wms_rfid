using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Allot.Interfaces;
using THOK.Wms.SignalR.Connection;
using THOK.Wms.SignalR.Common;
using THOK.Wms.SignalR.Model;
using System.Linq;
using THOK.Wms.DbModel;
using System;

namespace THOK.Wms.SignalR.Allot.Service
{
    public class OutBillAllotService : Notifier<AllotStockInConnection>, IOutBillAllotService
    {
        [Dependency]
        public IStorageLocker Locker { get; set; }

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
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        public void Allot(string connectionId, Model.ProgressState ps, System.Threading.CancellationToken cancellationToken, string billNo, string[] areaCodes)
        {
            Locker.LockKey = billNo;
            ConnectionId = connectionId;
            ps.State = StateType.Start;
            NotifyConnection(ps.Clone());

            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();

            OutBillMaster billMaster = outBillMasterQuery.Single(b => b.BillNo == billNo);

            if (!CheckAndLock(billMaster, ps)) { return; }

            //选择未分配的细单；
            var billDetails = billMaster.OutBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0);
            //选择当前订单操作目标仓库；
            var storages = storageQuery.Where(s => s.Cell.WarehouseCode == billMaster.WarehouseCode);

            if (areaCodes.Length > 0)
            {
                //选择指定库区；
                storages = storages.Where(s => areaCodes.Any(a => a == s.Cell.AreaCode));
            }

            foreach (var billDetail in billDetails.ToArray())
            {
                //分配条烟；
                var ss = storages.Where(s=>s.ProductCode == billDetail.ProductCode
                                            && s.Cell.Area.AreaType =="3")
                                 .OrderBy(s=>s.StorageTime);

            }

            cellQuery.Select(c => c.Storages.Where(s => s.LockTag == billNo).Select(s => s))
                     .AsParallel().ForAll(s => s.AsParallel().ForAll(i => i.LockTag = string.Empty));
            billMaster.LockTag = string.Empty;
            billMaster.Status = "3";
            CellRepository.SaveChanges();

            if (billMaster.OutBillDetails.Any(i => i.BillQuantity - i.AllotQuantity > 0))
            {
                ps.State = StateType.Warning;
                ps.Errors.Add("分配未全部完成，没有储位可分配！");
                NotifyConnection(ps.Clone());
            }
            else
            {
                ps.State = StateType.Info;
                ps.Messages.Add("分配完成!");
                NotifyConnection(ps.Clone());
            }
        }

        private bool CheckAndLock(OutBillMaster billMaster,ProgressState ps)
        {
            if ((new string[] { "1" }).Any(s => s == billMaster.Status))
            {
                ps.State = StateType.Info;
                ps.Messages.Add("当前订单未审核，不可以进行分配！");
                NotifyConnection(ps.Clone());
                return false;
            }

            if ((new string[] { "4", "5", "6" }).Any(s => s == billMaster.Status))
            {
                ps.State = StateType.Info;
                ps.Messages.Add("分配已确认生效不能再分配！");
                NotifyConnection(ps.Clone());
                return false;
            }

            if (!string.IsNullOrEmpty(billMaster.LockTag))
            {
                ps.State = StateType.Error;
                ps.Errors.Add("当前订单被锁定不可以进行分配！");
                NotifyConnection(ps.Clone());
                return false;
            }
            else
            {
                try
                {
                    billMaster.LockTag = ConnectionId;
                    OutBillMasterRepository.SaveChanges();
                    ps.Messages.Add("完成锁定当前订单");
                    NotifyConnection(ps.Clone());
                    return true;
                }
                catch (Exception)
                {
                    ps.State = StateType.Error;
                    ps.Errors.Add("锁定当前订单失败不可以进行分配！");
                    NotifyConnection(ps.Clone());
                    return false;
                }
            }
        }
    }
}
