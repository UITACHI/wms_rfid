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
                var dailyBalanceQuery = DailyBalanceRepository.GetQueryable().AsEnumerable();
                
                DateTime dt1 = Convert.ToDateTime(settleDate);
                var dailyBalance = dailyBalanceQuery.Where(d => d.SettleDate < dt1)
                                           .OrderByDescending(d=>d.SettleDate)
                                           .FirstOrDefault();
                string t = dailyBalance != null ? dailyBalance.SettleDate.ToString("yyyy-MM-dd") : "";

                var query = inQuery.Where(a => a.InBillMaster.WarehouseCode == warehouseCode
                                              && a.InBillMaster.BillDate.ToString("yyyy-MM-dd") == settleDate
                                  ).Select(a => new
                {
                    BillDate = a.InBillMaster.BillDate.ToString("yyyy-MM-dd"),
                    WarehouseCode = a.InBillMaster.Warehouse.WarehouseCode,
                    ProductCode = a.ProductCode,
                    UnitCode = a.Product.UnitCode,
                    Beginning = decimal.Zero,
                    EntryAmount = a.RealQuantity,
                    DeliveryAmount = decimal.Zero,
                    ProfitAmount = decimal.Zero,
                    LossAmount = decimal.Zero,
                    Ending = decimal.Zero
                }).Union(outQuery.Where(a => a.OutBillMaster.WarehouseCode == warehouseCode
                                            && a.OutBillMaster.BillDate.ToString("yyyy-MM-dd") == settleDate
                                ).Select(a => new
                {
                    BillDate = a.OutBillMaster.BillDate.ToString("yyyy-MM-dd"),
                    WarehouseCode = a.OutBillMaster.Warehouse.WarehouseCode,
                    ProductCode = a.ProductCode,
                    UnitCode = a.Product.UnitCode,
                    Beginning = decimal.Zero,
                    EntryAmount = decimal.Zero,
                    DeliveryAmount = a.RealQuantity,
                    ProfitAmount = decimal.Zero,
                    LossAmount = decimal.Zero,
                    Ending = decimal.Zero
                })).Union(profitLossQuery.Where(a => a.ProfitLossBillMaster.WarehouseCode == warehouseCode
                                                    && a.ProfitLossBillMaster.BillDate.ToString("yyyy-MM-dd") == settleDate
                                        ).Select(a => new
                {
                    BillDate = a.ProfitLossBillMaster.BillDate.ToString("yyyy-MM-dd"),
                    WarehouseCode = a.ProfitLossBillMaster.Warehouse.WarehouseCode,
                    ProductCode = a.ProductCode,
                    UnitCode = a.Product.UnitCode,
                    Beginning = decimal.Zero,
                    EntryAmount = decimal.Zero,
                    DeliveryAmount = decimal.Zero,
                    ProfitAmount = a.Quantity > 0 ? Math.Abs(a.Quantity) : decimal.Zero,
                    LossAmount = a.Quantity < 0 ? Math.Abs(a.Quantity) : decimal.Zero,
                    Ending = decimal.Zero
                })).Union(dailyBalanceQuery.Where(d => d.WarehouseCode == warehouseCode
                                                      && d.SettleDate.ToString("yyyy-MM-dd") == t
                                                      && d.Ending > decimal.Zero
                                          ).Select(a=>new
                {
                    BillDate = a.SettleDate.ToString("yyyy-MM-dd"),
                    WarehouseCode = a.WarehouseCode,
                    ProductCode = a.ProductCode,
                    UnitCode = a.Product.UnitCode,
                    Beginning = a.Ending,
                    EntryAmount = decimal.Zero,
                    DeliveryAmount = decimal.Zero,
                    ProfitAmount = decimal.Zero,
                    LossAmount = decimal.Zero,
                    Ending = decimal.Zero
                }
                ));

                var newDailyBalance = query.GroupBy(a => new { a.BillDate, a.WarehouseCode, a.ProductCode, a.UnitCode })
                                    .Select(a => new DailyBalance
                {
                    SettleDate = Convert.ToDateTime(a.Key.BillDate),
                    WarehouseCode = a.Key.WarehouseCode,   
                    ProductCode = a.Key.ProductCode,
                    UnitCode = a.Key.UnitCode,
                    Beginning = a.Sum(d => d.Beginning),
                    EntryAmount = a.Sum(d => d.EntryAmount),
                    DeliveryAmount = a.Sum(d=>d.DeliveryAmount),
                    ProfitAmount = a.Sum(d => d.ProfitAmount),
                    LossAmount = a.Sum(d => d.LossAmount),
                    Ending = a.Sum(d => d.Beginning) + a.Sum(d => d.EntryAmount) - a.Sum(d => d.DeliveryAmount) + a.Sum(d => d.ProfitAmount) - a.Sum(d => d.LossAmount),
                }).ToArray();

                newDailyBalance.AsParallel().ForAll(b => b.ID = Guid.NewGuid());                
                foreach (var item in newDailyBalance)
                {
                    item.ID = Guid.NewGuid();
                    DailyBalanceRepository.Add(item);
                }
                DailyBalanceRepository.SaveChanges();

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
