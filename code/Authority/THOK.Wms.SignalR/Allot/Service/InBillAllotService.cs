using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;
using THOK.Wms.SignalR.Connection;
using System.Threading;
using THOK.Wms.SignalR.Model;
using THOK.Wms.SignalR.Common;

namespace THOK.Wms.SignalR.Allot.Service
{
    public class InBillAllotService : Notifier<AllotStockInConnection>, IInBillAllotService
    {
        [Dependency]
        public IStorageLocker Locker { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }

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

        public void Allot(string connectionId,ProgressState ps, CancellationToken cancellationToken,string billNo, string[] areaCodes)
        {
            Locker.LockKey = billNo;
            ConnectionId = connectionId;
            ps.State = StateType.Start;
            NotifyConnection(ps.Clone());

            IQueryable<InBillMaster> inBillMasterQuery = InBillMasterRepository.GetQueryable();
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();

            InBillMaster billMaster = inBillMasterQuery.Single(b => b.BillNo == billNo);

            if (!CheckAndLock(billMaster, ps)){return;}

            //选择未分配的细单；
            var billDetails = billMaster.InBillDetails.Where(b => (b.BillQuantity - b.AllotQuantity) > 0);
            //选择当前订单操作目标仓库；
            var cells = cellQuery.Where(c => c.WarehouseCode == billMaster.WarehouseCode); 
            if (areaCodes.Length > 0)
            {
                //选择指定库区；
                cells = cells.Where(c => areaCodes.Any(a => a == c.AreaCode));
            }
            else
            {
                cells = cells.Where(c => c.Area.AllotInOrder > 0);
            }
            
            //1：主库区；2：件烟区；
            //3；条烟区；4：暂存区；
            //5：备货区；6：残烟区；
            //7：罚烟区；8：虚拟区；
            //9：其他区；

            //排除 件烟区,条烟区 货位是单一存储的空货位；
            string [] areaTypes = new string []{"2","3"};
            var cellList1 = cells.Where(c => areaTypes.All(a => a != c.Area.AreaType)
                                            && c.IsSingle == "1"      
                                            && (c.Storages.Count==0
                                                    || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag) 
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                        ).ToList();

            //条烟区 货位是单一存储的货位（不必是空货位，因为条烟会多次存储到同一个货位）；
            areaTypes = new string[] {"3"};
            var cellList2 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                            && c.IsSingle == "1"      
                                        ).ToList();

            //件烟区 货位是单一存储的空货位； 
            areaTypes = new string[] {"2"};
            var cellList3 = cells.Where(c => areaTypes.Any(a => a == c.Area.AreaType)
                                            && c.IsSingle == "1"     
                                            && (c.Storages.Count == 0
                                                    || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag) 
                                                        && s.Quantity == 0
                                                        && s.InFrozenQuantity == 0
                                                    )
                                                )
                                        ).ToList();

            //非货位管理区
            var cellList4 = cells.Where(c => c.IsSingle == "0").ToList();


            //排除 件烟区，条烟区
            var cellQueryFromList1 = cellList1.Where(c => c.Storages.Count == 0
                                                || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag) 
                                                    && s.Quantity == 0 
                                                    && s.InFrozenQuantity == 0))
                                              .OrderBy(c=>c.Area.AllotInOrder);
            //条烟区
            var cellQueryFromList2 = cellList2.OrderBy(c => c.Area.AllotInOrder);

            //件烟区
            var cellQueryFromList3 = cellList3.Where(c => c.Storages.Count == 0
                                                || c.Storages.Any(s => string.IsNullOrEmpty(s.LockTag) 
                                                    && s.Quantity == 0
                                                    && s.InFrozenQuantity == 0))
                                              .OrderBy(c => c.Area.AllotInOrder);
            //非货位管理区
            var cellQueryFromList4 = cellList4.OrderBy(c => c.Area.AllotInOrder);
            
            foreach (var billDetail in billDetails.ToArray())
            {
                //分配预设当前卷烟的货位；
                var cs = cellQueryFromList1.Where(c => c.DefaultProductCode == billDetail.ProductCode);
                AllotPallet(billMaster, billDetail, cs, cancellationToken, ps);
                //分配没预设卷烟的货位；
                cs = cellQueryFromList1.Where(c => string.IsNullOrEmpty(c.DefaultProductCode));
                AllotPallet(billMaster, billDetail, cs, cancellationToken, ps);
                //分配预设其他卷烟的货位；
                cs = cellQueryFromList1.Where(c => c.DefaultProductCode != billDetail.ProductCode
                                                    && !string.IsNullOrEmpty(c.DefaultProductCode));
                AllotPallet(billMaster, billDetail, cs, cancellationToken,ps);

                //分配条烟到条烟区；
                cs = cellQueryFromList2.Where(c => c.DefaultProductCode == billDetail.ProductCode
                                                        || (c.Storages.Count == 1
                                                            && c.Storages.First().ProductCode == billDetail.ProductCode) 
                                                        );
                AllotBar(billMaster, billDetail, cs, cancellationToken, ps);
                //分配条烟到条烟区；
                if (cs.Count() == 0)
                {
                    cs = cellQueryFromList2.Where(c => string.IsNullOrEmpty(c.DefaultProductCode));
                    AllotBar(billMaster, billDetail, cs, cancellationToken, ps);
                }

                //分配未满一托盘的卷烟到件烟区；
                cs = cellQueryFromList3;
                if (cellQueryFromList2.Count() > 0)
                {
                    AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);
                }
                else
                {
                    AllotPieceAndBar(billMaster, billDetail, cs, cancellationToken, ps);
                }    

                //分配未满一托盘的卷烟到下层货架；
                cs = cellQueryFromList1.Where(c => c.Layer == 1);
                if (cellQueryFromList2.Count() > 0)
                {
                    AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);
                }
                else
                {
                    AllotPieceAndBar(billMaster, billDetail, cs, cancellationToken, ps);
                }

                //分配未分配卷烟到其他库区；
                cs = cellQueryFromList1;
                AllotPiece(billMaster, billDetail, cs, cancellationToken, ps);

                //分配未分配卷烟到其他非货位管理货位；
                while (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    var c = cellQueryFromList4.FirstOrDefault();
                    if (c != null)
                    {
                        decimal allotQuantity = c.MaxQuantity * billDetail.Product.Unit.Count;
                        decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                        allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                        Allot(billMaster, billDetail, c, Locker.LockEmpty(c), allotQuantity, ps);
                    }
                    else break;
                }
            }            
            
            cellQuery.Select(c => c.Storages.Where(s => s.LockTag == billNo).Select(s => s))
                     .AsParallel().ForAll(s=>s.AsParallel().ForAll(i=>i.LockTag = string.Empty));
            billMaster.LockTag = string.Empty;
            billMaster.Status = "3";
            CellRepository.SaveChanges();

            if (billMaster.InBillDetails.Any(i => i.BillQuantity - i.AllotQuantity > 0))
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

        //分配件烟
        private void AllotPiece(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal allotQuantity = c.MaxQuantity * billDetail.Product.Unit.Count;
                    decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                        / billDetail.Product.Unit.Count)
                        * billDetail.Product.Unit.Count;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    Allot(billMaster, billDetail, c, Locker.LockEmpty(c), allotQuantity, ps); 
                }
                else break;
            }
        }

        //分配件烟，条烟
        private void AllotPieceAndBar(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal allotQuantity = c.MaxQuantity * billDetail.Product.Unit.Count;
                    decimal billQuantity = billDetail.BillQuantity - billDetail.AllotQuantity;
                    allotQuantity = allotQuantity < billQuantity ? allotQuantity : billQuantity;
                    Allot(billMaster, billDetail, c, Locker.LockEmpty(c), allotQuantity, ps); 
                }
                else break;
            }
        }

        //分配条烟
        private void AllotBar(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {
            foreach (var c in cs)
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal billQuantity = (billDetail.BillQuantity - billDetail.AllotQuantity) % billDetail.Product.Unit.Count;
                    if (billQuantity > 0)
                    {
                        Allot(billMaster, billDetail, c, Locker.LockBar(c, billDetail.Product), billQuantity, ps);
                    }
                    else break;
                }
                else break;
            }
        }

        //分配整盘
        private void AllotPallet(InBillMaster billMaster, InBillDetail billDetail, IEnumerable<Cell> cs, CancellationToken cancellationToken, ProgressState ps)
        {       
            foreach (var c in cs)
            {
                if (!cancellationToken.IsCancellationRequested && (billDetail.BillQuantity - billDetail.AllotQuantity) > 0)
                {
                    decimal allotQuantity = c.MaxQuantity * billDetail.Product.Unit.Count;
                    decimal billQuantity = Math.Floor((billDetail.BillQuantity - billDetail.AllotQuantity)
                        / billDetail.Product.Unit.Count)
                        * billDetail.Product.Unit.Count;
                    if (billQuantity >= allotQuantity)
                    {
                        Allot(billMaster, billDetail, c, Locker.LockEmpty(c), allotQuantity, ps);
                    }
                    else break;
                }
                else break;
            }
        }

        //检查主表并加锁
        private bool CheckAndLock(InBillMaster billMaster, ProgressState ps)
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
                    InBillMasterRepository.SaveChanges();
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

        private void Allot(InBillMaster billMaster, InBillDetail billDetail, Cell cell, Storage storage, decimal allotQuantity,ProgressState ps)
        {
            if (storage != null && allotQuantity > 0)
            {
                InBillAllot billAllot = null;
                billDetail.AllotQuantity += allotQuantity;                
                storage.ProductCode = billDetail.ProductCode;
                storage.LockTag = billDetail.BillNo;
                storage.InFrozenQuantity += allotQuantity;

                billAllot = new InBillAllot()
                {
                    BillNo = billMaster.BillNo,
                    InBillDetailId = billDetail.ID,
                    ProductCode = billDetail.ProductCode,
                    CellCode = cell.CellCode,
                    StorageCode = storage.StorageCode,
                    UnitCode = billDetail.UnitCode,
                    AllotQuantity = allotQuantity,
                    RealQuantity = 0,
                    Status = "0"
                };
                billMaster.InBillAllots.Add(billAllot);
                StorageRepository.SaveChanges();

                decimal sumBillQuantity = billMaster.InBillDetails.Sum(d => d.BillQuantity);
                decimal sumAllotQuantity = billMaster.InBillDetails.Sum(d => d.AllotQuantity);

                decimal sumBillProductQuantity = billMaster.InBillDetails.Where(d => d.ProductCode == billDetail.ProductCode)
                                                                         .Sum(d => d.BillQuantity);
                decimal sumAllotProductQuantity = billMaster.InBillDetails.Where(d => d.ProductCode == billDetail.ProductCode)
                                                                          .Sum(d => d.AllotQuantity);

                ps.State = StateType.Processing;
                ps.TotalProgressName = "分配入库单：" + billMaster.BillNo;
                ps.TotalProgressValue = (int)(sumAllotQuantity / sumBillQuantity * 100);
                ps.CurrentProgressName = "分配卷烟：" + billDetail.Product.ProductName;
                ps.CurrentProgressValue = (int)(sumAllotProductQuantity / sumBillProductQuantity * 100);
                NotifyConnection(ps.Clone());
            }
        }
    }
}
