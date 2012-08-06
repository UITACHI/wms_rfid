using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class DailyBalanceService : ServiceBase<DailyBalance>, IDailyBalanceService
    {
        [Dependency]
        public IDailyBalanceRepository DailyBalanceRepository { get; set; }

        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public IProfitLossBillDetailRepository ProfitLossBillDetailRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
        
        #region IDailyBalanceService 成员

        public object GetDetails(int page, int rows, string beginDate, string endDate, string warehouseCode)
        {
            IQueryable<DailyBalance> dailyBalanceQuery = DailyBalanceRepository.GetQueryable();
            var dailyBalance = dailyBalanceQuery.Where(c => c.WarehouseCode.Contains(warehouseCode))
                                                .OrderBy(c => c.SettleDate).AsEnumerable()
                                                .GroupBy(c => c.SettleDate)
                                                .Select(c => new
                                                {
                                                    SettleDate = c.Key.ToString("yyyy-MM-dd"),
                                                    WarehouseCode = warehouseCode == "" ? "" : c.Max(p => p.WarehouseCode),
                                                    WarehouseName = warehouseCode == "" ? "全部仓库" : c.Max(p => p.Warehouse.WarehouseName),
                                                    Beginning = c.Sum(p => p.Beginning),
                                                    EntryAmount = c.Sum(p => p.EntryAmount),
                                                    DeliveryAmount = c.Sum(p => p.DeliveryAmount),
                                                    ProfitAmount = c.Sum(p => p.ProfitAmount),
                                                    LossAmount = c.Sum(p => p.LossAmount),
                                                    Ending = c.Sum(p => p.Ending)
                                                });
            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                dailyBalance = dailyBalance.Where(i => Convert.ToDateTime(i.SettleDate) >= begin);
            }

            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate);
                dailyBalance = dailyBalance.Where(i => Convert.ToDateTime(i.SettleDate) <= end);
            }
            int total = dailyBalance.Count();
            dailyBalance = dailyBalance.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = dailyBalance.ToArray() };
        }

        public object GetInfoDetails(int page, int rows, string warehouseCode, string settleDate)
        {
            IQueryable<DailyBalance> dailyBalanceQuery = DailyBalanceRepository.GetQueryable();
            var query = dailyBalanceQuery.Where(i => i.WarehouseCode.Contains(warehouseCode)
                                       ).OrderBy(i => i.SettleDate).OrderBy(i => i.Warehouse.WarehouseName
                                       ).AsEnumerable().Select(i => new
                                       {
                                           SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                           i.ProductCode,
                                           i.Product.ProductName,
                                           i.UnitCode,
                                           i.Unit.UnitName,
                                           i.WarehouseCode,
                                           i.Warehouse.WarehouseName,
                                           i.Beginning,
                                           i.EntryAmount,
                                           i.DeliveryAmount,
                                           i.ProfitAmount,
                                           i.LossAmount,
                                           i.Ending
                                       });
            if (!settleDate.Equals(string.Empty))
            {
                DateTime date = Convert.ToDateTime(settleDate);
                query = query.Where(i => Convert.ToDateTime(i.SettleDate) == date);
            }
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = query.ToArray() };
        }

        public object GetDailyBalanceInfos(int page, int rows, string warehouseCode, string settleDate)
        {
            var inQuery = InBillDetailRepository.GetQueryable().AsEnumerable();
            var outQuery = OutBillDetailRepository.GetQueryable().AsEnumerable();
            var profitLossQuery = ProfitLossBillDetailRepository.GetQueryable().AsEnumerable();

            var query = inQuery.Where(a => a.InBillMaster.WarehouseCode.Contains(warehouseCode)).Select(a => new
            {
                BillDate = a.InBillMaster.BillDate.ToString("yyyy-MM-dd"),
                a.InBillMaster.Warehouse.WarehouseCode,
                a.InBillMaster.Warehouse.WarehouseName,
                a.BillNo,
                a.InBillMaster.BillType.BillTypeCode,
                a.InBillMaster.BillType.BillTypeName,
                a.ProductCode,
                a.Product.ProductName,
                RealQuantity=a.RealQuantity,
                Beginning=0.00.ToString(),
                EntryAmount = a.RealQuantity.ToString(),
                ProfitAmount = 0.00.ToString(),
                LossAmount = 0.00.ToString(),
                Ending = 0.00.ToString(),
                a.Unit.UnitName
            }).Union(outQuery.Where(a => a.OutBillMaster.WarehouseCode.Contains(warehouseCode)).Select(a => new
            {
                BillDate = a.OutBillMaster.BillDate.ToString("yyyy-MM-dd"),
                a.OutBillMaster.Warehouse.WarehouseCode,
                a.OutBillMaster.Warehouse.WarehouseName,
                a.BillNo,
                a.OutBillMaster.BillType.BillTypeCode,
                a.OutBillMaster.BillType.BillTypeName,
                a.ProductCode,
                a.Product.ProductName,
                RealQuantity = a.RealQuantity,
                Beginning = 0.00.ToString(),
                EntryAmount = 0.00.ToString(),
                ProfitAmount = 0.00.ToString(),
                LossAmount = 0.00.ToString(),
                Ending = 0.00.ToString(),
                a.Unit.UnitName
            })).Union(profitLossQuery.Where(a => a.ProfitLossBillMaster.WarehouseCode.Contains(warehouseCode)).Select(a => new
            {
                BillDate = a.ProfitLossBillMaster.BillDate.ToString("yyyy-MM-dd"),
                a.ProfitLossBillMaster.Warehouse.WarehouseCode,
                a.ProfitLossBillMaster.Warehouse.WarehouseName,
                a.BillNo,
                a.ProfitLossBillMaster.BillType.BillTypeCode,
                a.ProfitLossBillMaster.BillType.BillTypeName,
                a.ProductCode,
                a.Product.ProductName,
                RealQuantity = a.Quantity,
                Beginning = 0.00.ToString(),
                EntryAmount = 0.00.ToString(),
                ProfitAmount =a.Quantity> 0 ? a.Quantity.ToString() : 0.00.ToString(),
                LossAmount = a.Quantity < 0 ? (-a.Quantity).ToString() : 0.00.ToString(),
                Ending = 0.00.ToString(),
                a.Unit.UnitName
            }));

            if (!settleDate.Equals(string.Empty))
            {
                DateTime date = Convert.ToDateTime(settleDate);
                query = query.Where(i => Convert.ToDateTime(i.BillDate) == date);
            }
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = query.ToArray() };
        }

        public Boolean DoDailyBalance(string warehouseCode, string settleDate)
        {
            try
            {
                var inQuery = InBillDetailRepository.GetQueryable().AsEnumerable();
                var outQuery = OutBillDetailRepository.GetQueryable().AsEnumerable();
                var profitLossQuery = ProfitLossBillDetailRepository.GetQueryable().AsEnumerable();

                var query = inQuery.Where(a => a.InBillMaster.WarehouseCode.Contains(warehouseCode)).Select(a => new
                {
                    BillDate = a.InBillMaster.BillDate.ToString("yyyy-MM-dd"),
                    a.InBillMaster.Warehouse.WarehouseCode,
                    a.InBillMaster.Warehouse.WarehouseName,
                    a.BillNo,
                    a.InBillMaster.BillType.BillTypeCode,
                    a.InBillMaster.BillType.BillTypeName,
                    a.ProductCode,
                    a.Product.ProductName,
                    RealQuantity = a.RealQuantity,
                    Beginning = 0.00.ToString(),
                    EntryAmount = a.RealQuantity.ToString(),
                    ProfitAmount = 0.00.ToString(),
                    LossAmount = 0.00.ToString(),
                    Ending = 0.00.ToString(),
                    a.Unit.UnitName
                }).Union(outQuery.Where(a => a.OutBillMaster.WarehouseCode.Contains(warehouseCode)).Select(a => new
                {
                    BillDate = a.OutBillMaster.BillDate.ToString("yyyy-MM-dd"),
                    a.OutBillMaster.Warehouse.WarehouseCode,
                    a.OutBillMaster.Warehouse.WarehouseName,
                    a.BillNo,
                    a.OutBillMaster.BillType.BillTypeCode,
                    a.OutBillMaster.BillType.BillTypeName,
                    a.ProductCode,
                    a.Product.ProductName,
                    RealQuantity = a.RealQuantity,
                    Beginning = 0.00.ToString(),
                    EntryAmount = 0.00.ToString(),
                    ProfitAmount = 0.00.ToString(),
                    LossAmount = 0.00.ToString(),
                    Ending = 0.00.ToString(),
                    a.Unit.UnitName
                })).Union(profitLossQuery.Where(a => a.ProfitLossBillMaster.WarehouseCode.Contains(warehouseCode)).Select(a => new
                {
                    BillDate = a.ProfitLossBillMaster.BillDate.ToString("yyyy-MM-dd"),
                    a.ProfitLossBillMaster.Warehouse.WarehouseCode,
                    a.ProfitLossBillMaster.Warehouse.WarehouseName,
                    a.BillNo,
                    a.ProfitLossBillMaster.BillType.BillTypeCode,
                    a.ProfitLossBillMaster.BillType.BillTypeName,
                    a.ProductCode,
                    a.Product.ProductName,
                    RealQuantity = a.Quantity,
                    Beginning = 0.00.ToString(),
                    EntryAmount = 0.00.ToString(),
                    ProfitAmount = a.Quantity > 0 ? a.Quantity.ToString() : 0.00.ToString(),
                    LossAmount = a.Quantity < 0 ? (-a.Quantity).ToString() : 0.00.ToString(),
                    Ending = 0.00.ToString(),
                    a.Unit.UnitName
                }));

                if (!settleDate.Equals(string.Empty))
                {
                    DateTime date = Convert.ToDateTime(settleDate);
                    query = query.Where(i => Convert.ToDateTime(i.BillDate) == date);
                }
                var Newquery = query.OrderBy(a => a.ProductName).AsEnumerable().GroupBy(a => a.ProductCode).Select(a => new
                {
                    BillDate = a.Max(s => s.BillDate.ToString()),
                    WarehouseCode = a.Max(s => s.WarehouseCode.ToString()),
                    WarehouseName = a.Max(s => s.WarehouseName.ToString()),
                    ProductCode = a.Max(s => s.ProductCode.ToString()),
                    ProductName = a.Max(s => s.ProductName.ToString()),
                    RealQuantity = a.Max(s => s.RealQuantity.ToString()),
                    Beginning = a.Max(s => s.Beginning.ToString()),
                    EntryAmount = a.Max(s => s.EntryAmount.ToString()),
                    ProfitAmount = a.Max(s => s.ProfitAmount.ToString()),
                    LossAmount = a.Max(s => s.LossAmount.ToString()),
                    Ending = a.Max(s => s.Ending.ToString()),
                    UnitName = a.Max(s => s.UnitName.ToString()),
                });
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

    }
}
