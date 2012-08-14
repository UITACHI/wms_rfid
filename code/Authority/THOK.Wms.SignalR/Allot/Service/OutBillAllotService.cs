using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Allot.Interfaces;
using THOK.Wms.SignalR.Connection;
using THOK.Wms.SignalR.Common;
using THOK.Wms.SignalR.Model;
using System.Linq;
using THOK.Wms.DbModel;
using System;
using Entities.Extensions;

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
            var billDetails = billMaster.OutBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0).ToArray();
            //选择当前订单操作目标仓库；
            var storages = storageQuery.Where(s => s.Cell.WarehouseCode == billMaster.WarehouseCode);
            if (areaCodes.Length > 0)
            {
                //选择指定库区；
                storages = storages.Where(s => areaCodes.Any(a => a == s.Cell.AreaCode));
            }
            else
            {
                storages = storages.Where(s => s.Cell.Area.AllotOutOrder > 0);
            }
            storages = storages.Where(s => string.IsNullOrEmpty(s.LockTag)
                                            && s.Quantity - s.OutFrozenQuantity > 0);

            foreach (var billDetail in billDetails)
            {
                //1：主库区 1；2：件烟区 2；
                //3；条烟区 3；4：暂存区 4；
                //5：备货区 0；6：残烟区 0；
                //7：罚烟区 0；8：虚拟区 0；
                //9：其他区 0；

                //分配整盘；排除 件烟区 条烟区
                string[] areaTypes = new string[] { "2", "3" };
                var ss = storages.Where(s=>areaTypes.All(a => a != s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode)
                                 .OrderBy(s=>s.StorageTime)
                                 .OrderBy(s=>s.Cell.Area.AllotOutOrder);
                AllotPallet(billMaster, billDetail, ss, cancellationToken, ps);

                //分配件烟；件烟区 
                areaTypes = new string[] { "2"};
                ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotPiece(billMaster, billDetail, ss, cancellationToken, ps);

                //分配件烟 (下层储位)；排除 件烟区 条烟区 
                areaTypes = new string[] { "2", "3" };
                ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode
                                            && s.Cell.Layer == 1)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotPiece(billMaster, billDetail, ss, cancellationToken, ps);

                //分配件烟 (非下层储位)；排除 件烟区 条烟区 
                areaTypes = new string[] { "2", "3" };
                ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode
                                            && s.Cell.Layer != 1)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotPiece(billMaster, billDetail, ss, cancellationToken, ps);

                //分配条烟；条烟区
                areaTypes = new string[] { "3" };
                ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotBar(billMaster, billDetail, ss, cancellationToken, ps);

                //分配条烟；件烟区
                areaTypes = new string[] { "2" };
                ss = storages.Where(s => areaTypes.Any(a => a == s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotBar(billMaster, billDetail, ss, cancellationToken, ps);

                //分配条烟 (下层储位)；排除 件烟区 条烟区 
                areaTypes = new string[] { "2", "3" };
                ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode
                                            && s.Cell.Layer == 1)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotBar(billMaster, billDetail, ss, cancellationToken, ps);

                //分配条烟 (非下层储位)；排除 件烟区 条烟区 
                areaTypes = new string[] { "2", "3" };
                ss = storages.Where(s => areaTypes.All(a => a != s.Cell.Area.AreaType)
                                            && s.ProductCode == billDetail.ProductCode
                                            && s.Cell.Layer != 1)
                                 .OrderBy(s => s.StorageTime)
                                 .OrderBy(s => s.Cell.Area.AllotOutOrder);
                AllotBar(billMaster, billDetail, ss, cancellationToken, ps);
            }

            string billno = billMaster.BillNo;
            if (billMaster.OutBillDetails.Any(i => i.BillQuantity - i.AllotQuantity > 0))
            {
                ps.State = StateType.Warning;
                ps.Errors.Add("分配未全部完成，没有储位可分配！");
                NotifyConnection(ps.Clone());

                OutBillMasterRepository.GetObjectSet()
                    .UpdateEntity(i => i.BillNo == billno,
                    i => new OutBillMaster() { LockTag = "" });
            }
            else
            {
                ps.State = StateType.Info;
                ps.Messages.Add("分配完成,开始保存请稍候!");
                NotifyConnection(ps.Clone());

                billMaster.Status = "3";
                try
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        billMaster.LockTag = string.Empty;
                        CellRepository.SaveChanges();
                        ps.State = StateType.Info;
                        ps.Messages.Clear();
                        ps.Messages.Add("分配成功!");
                        NotifyConnection(ps.Clone());
                    }
                }
                catch (Exception e)
                {
                    ps.State = StateType.Error;
                    ps.Messages.Add("保存失败，详情：" + e.Message);
                    NotifyConnection(ps.Clone());
                }
                finally
                {
                    OutBillMasterRepository.GetObjectSet()
                        .UpdateEntity(i => i.BillNo == billno,
                        i => new OutBillMaster() { LockTag = "" });
                }
            }
        }

        private void AllotBar(OutBillMaster billMaster, OutBillDetail billDetail, IOrderedQueryable<Storage> ss, System.Threading.CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var s in ss.ToArray())
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    Allot(billMaster, billDetail, s, Locker.LockNoEmptyStorage(s, billDetail.Product), allotQuantity, ps);
                }
                else break;
            }
        }

        private void AllotPiece(OutBillMaster billMaster, OutBillDetail billDetail, IOrderedQueryable<Storage> ss, System.Threading.CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var s in ss.ToArray())
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                        / billDetail.Product.Unit.Count)
                        * billDetail.Product.Unit.Count;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    Allot(billMaster, billDetail, s, Locker.LockNoEmptyStorage(s, billDetail.Product), allotQuantity, ps);
                }
                else break;
            }
        }

        private void AllotPallet(OutBillMaster billMaster, OutBillDetail billDetail, IOrderedQueryable<Storage> ss, System.Threading.CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var s in ss.ToArray())
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal allotQuantity = s.Quantity - s.OutFrozenQuantity;
                    decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                        / billDetail.Product.Unit.Count)
                        * billDetail.Product.Unit.Count;
                    if (billQuantity >= allotQuantity)
                    {
                        Allot(billMaster, billDetail, s, Locker.LockNoEmptyStorage(s,billDetail.Product), allotQuantity, ps);
                    }
                    else break;
                }
                else break;
            }
        }

        private void Allot(OutBillMaster billMaster, OutBillDetail billDetail, Storage storage, Storage p, decimal allotQuantity, ProgressState ps)
        {
            if (storage != null && allotQuantity > 0)
            {
                OutBillAllot billAllot = null;
                billDetail.AllotQuantity += allotQuantity;
                storage.OutFrozenQuantity += allotQuantity;

                billAllot = new OutBillAllot()
                {
                    BillNo = billMaster.BillNo,
                    OutBillDetailId = billDetail.ID,
                    ProductCode = billDetail.ProductCode,
                    CellCode = storage.CellCode,
                    StorageCode = storage.StorageCode,
                    UnitCode = billDetail.UnitCode,
                    AllotQuantity = allotQuantity,
                    RealQuantity = 0,
                    Status = "0"
                };
                billMaster.OutBillAllots.Add(billAllot);

                decimal sumBillQuantity = billMaster.OutBillDetails.Sum(d => d.BillQuantity);
                decimal sumAllotQuantity = billMaster.OutBillDetails.Sum(d => d.AllotQuantity);

                decimal sumBillProductQuantity = billMaster.OutBillDetails.Where(d => d.ProductCode == billDetail.ProductCode)
                                                                         .Sum(d => d.BillQuantity);
                decimal sumAllotProductQuantity = billMaster.OutBillDetails.Where(d => d.ProductCode == billDetail.ProductCode)
                                                                          .Sum(d => d.AllotQuantity);

                ps.State = StateType.Processing;
                ps.TotalProgressName = "分配出库单：" + billMaster.BillNo;
                ps.TotalProgressValue = (int)(sumAllotQuantity / sumBillQuantity * 100);
                ps.CurrentProgressName = "分配卷烟：" + billDetail.Product.ProductName;
                ps.CurrentProgressValue = (int)(sumAllotProductQuantity / sumBillProductQuantity * 100);
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
