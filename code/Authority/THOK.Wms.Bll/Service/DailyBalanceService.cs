using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Transactions;

namespace THOK.Wms.Bll.Service
{
    public class DailyBalanceService : ServiceBase<DailyBalance>, IDailyBalanceService
    {
        [Dependency]
        public IDailyBalanceRepository DailyBalanceRepository { get; set; }
        [Dependency]
        public IUnitRepository UnitRepository { get; set; }
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
        
        public object GetDetails(int page, int rows, string beginDate, string endDate, string warehouseCode,string unitType)
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
                                        UnitName = "",
                                        Beginning = c.Sum(p => p.Beginning),
                                        EntryAmount = c.Sum(p => p.EntryAmount),
                                        DeliveryAmount = c.Sum(p => p.DeliveryAmount),
                                        ProfitAmount = c.Sum(p => p.ProfitAmount),
                                        LossAmount = c.Sum(p => p.LossAmount),
                                        Ending = c.Sum(p => p.Ending)
                                    });

            string unitName = "件（标准）";
            decimal count = 10000;
            if (unitType == "2")
            {
                unitName = "条（标准）";
                count = 200;
            }

            dailyBalance = dailyBalance.Select(d => new
            {
                SettleDate = d.SettleDate,
                WarehouseCode = d.WarehouseCode,
                WarehouseName = d.WarehouseName,
                UnitName = unitName,
                Beginning = d.Beginning / count,
                EntryAmount = d.EntryAmount / count,
                DeliveryAmount = d.DeliveryAmount / count,
                ProfitAmount = d.ProfitAmount / count,
                LossAmount = d.LossAmount / count,
                Ending = d.Ending / count
            }
            );

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

        public object GetInfoDetails(int page, int rows, string warehouseCode, string settleDate,string unitType)
        {
            IQueryable<DailyBalance> dailyBalanceQuery = DailyBalanceRepository.GetQueryable();
            var query = dailyBalanceQuery.AsEnumerable()
                                         .Where(i => i.WarehouseCode.Contains(warehouseCode)
                                                    && i.SettleDate.ToString("yyyy-MM-dd") == settleDate
                                                )
                                         .OrderBy(i => i.SettleDate)
                                         .OrderBy(i => i.Warehouse.WarehouseName)
                                         .AsEnumerable().Select(i => new
                                         {
                                             SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                             i.ProductCode,
                                             i.Product.ProductName,
                                             i.UnitCode,
                                             i.Unit.UnitName,
                                             i.WarehouseCode,
                                             i.Warehouse.WarehouseName,
                                             Beginning = i.Beginning/i.Unit.Count,
                                             EntryAmount = i.EntryAmount/i.Unit.Count,
                                             DeliveryAmount = i.DeliveryAmount/i.Unit.Count,
                                             ProfitAmount = i.ProfitAmount/i.Unit.Count,
                                             LossAmount = i.LossAmount/i.Unit.Count,
                                             Ending = i.Ending/i.Unit.Count
                                         });

            //标准件（万支）
            if (unitType == "1")
            {
                query = dailyBalanceQuery.AsEnumerable()
                                         .Where(i => i.WarehouseCode.Contains(warehouseCode)
                                                    && i.SettleDate.ToString("yyyy-MM-dd") == settleDate
                                                )
                                         .OrderBy(i => i.SettleDate)
                                         .OrderBy(i => i.Warehouse.WarehouseName)
                                         .AsEnumerable().Select(i => new
                                         {
                                             SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                             i.ProductCode,
                                             i.Product.ProductName,
                                             UnitCode = "",
                                             UnitName = "标准件（万支）",
                                             i.WarehouseCode,
                                             i.Warehouse.WarehouseName,
                                             Beginning = i.Beginning / 10000,
                                             EntryAmount = i.EntryAmount / 10000,
                                             DeliveryAmount = i.DeliveryAmount / 10000,
                                             ProfitAmount = i.ProfitAmount / 10000,
                                             LossAmount = i.LossAmount / 10000,
                                             Ending = i.Ending / 10000
                                         });
            }

            //标准条（200支）
            if (unitType == "2")
            {
                query = dailyBalanceQuery.AsEnumerable()
                                         .Where(i => i.WarehouseCode.Contains(warehouseCode)
                                                    && i.SettleDate.ToString("yyyy-MM-dd") == settleDate
                                                )
                                         .OrderBy(i => i.SettleDate)
                                         .OrderBy(i => i.Warehouse.WarehouseName)
                                         .AsEnumerable().Select(i => new
                                         {
                                             SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                             i.ProductCode,
                                             i.Product.ProductName,
                                             UnitCode = "",
                                             UnitName = "标准条（200支）",
                                             i.WarehouseCode,
                                             i.Warehouse.WarehouseName,
                                             Beginning = i.Beginning / 200,
                                             EntryAmount = i.EntryAmount / 200,
                                             DeliveryAmount = i.DeliveryAmount / 200,
                                             ProfitAmount = i.ProfitAmount / 200,
                                             LossAmount = i.LossAmount / 200,
                                             Ending = i.Ending / 200
                                         });
            }

            //自然件
            if (unitType == "3")
            {
                query = dailyBalanceQuery.AsEnumerable()
                                         .Where(i => i.WarehouseCode.Contains(warehouseCode)
                                                    && i.SettleDate.ToString("yyyy-MM-dd") == settleDate
                                                )
                                         .OrderBy(i => i.SettleDate)
                                         .OrderBy(i => i.Warehouse.WarehouseName)
                                         .AsEnumerable().Select(i => new
                                         {
                                             SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                             i.ProductCode,
                                             i.Product.ProductName,
                                             i.Product.UnitList.Unit01.UnitCode,
                                             i.Product.UnitList.Unit01.UnitName,
                                             i.WarehouseCode,
                                             i.Warehouse.WarehouseName,
                                             Beginning = i.Beginning / i.Product.UnitList.Unit01.Count,
                                             EntryAmount = i.EntryAmount / i.Product.UnitList.Unit01.Count,
                                             DeliveryAmount = i.DeliveryAmount / i.Product.UnitList.Unit01.Count,
                                             ProfitAmount = i.ProfitAmount / i.Product.UnitList.Unit01.Count,
                                             LossAmount = i.LossAmount / i.Product.UnitList.Unit01.Count,
                                             Ending = i.Ending / i.Product.UnitList.Unit01.Count
                                         });
            }

            //自然条
            if (unitType == "4")
            {
                query = dailyBalanceQuery.AsEnumerable()
                                         .Where(i => i.WarehouseCode.Contains(warehouseCode)
                                                    && i.SettleDate.ToString("yyyy-MM-dd") == settleDate
                                                )
                                         .OrderBy(i => i.SettleDate)
                                         .OrderBy(i => i.Warehouse.WarehouseName)
                                         .AsEnumerable().Select(i => new
                                         {
                                             SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                             i.ProductCode,
                                             i.Product.ProductName,
                                             i.Product.UnitList.Unit02.UnitCode,
                                             i.Product.UnitList.Unit02.UnitName,
                                             i.WarehouseCode,
                                             i.Warehouse.WarehouseName,
                                             Beginning = i.Beginning / i.Product.UnitList.Unit02.Count,
                                             EntryAmount = i.EntryAmount / i.Product.UnitList.Unit02.Count,
                                             DeliveryAmount = i.DeliveryAmount / i.Product.UnitList.Unit02.Count,
                                             ProfitAmount = i.ProfitAmount / i.Product.UnitList.Unit02.Count,
                                             LossAmount = i.LossAmount / i.Product.UnitList.Unit02.Count,
                                             Ending = i.Ending / i.Product.UnitList.Unit02.Count
                                         });
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

        public Boolean DoDailyBalance(string warehouseCode, string settleDate,ref string errorInfo)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var inQuery = InBillDetailRepository.GetQueryable().AsEnumerable();
                    var outQuery = OutBillDetailRepository.GetQueryable().AsEnumerable();
                    var profitLossQuery = ProfitLossBillDetailRepository.GetQueryable().AsEnumerable();
                    var dailyBalanceQuery = DailyBalanceRepository.GetQueryable().AsEnumerable();

                    DateTime dt1 = Convert.ToDateTime(settleDate);

                    if (DateTime.Now < dt1)
                    {
                        errorInfo = "选择日结日期大于当前日期，不可以进行日结！";
                        return false;
                    }
                    var dailyBalance = dailyBalanceQuery.Where(d => d.SettleDate < dt1)
                                               .OrderByDescending(d => d.SettleDate)
                                               .FirstOrDefault();
                    string t = dailyBalance != null ? dailyBalance.SettleDate.ToString("yyyy-MM-dd") : "";

                    var oldDailyBalance = dailyBalanceQuery.Where(d => (d.WarehouseCode == warehouseCode
                                                                         || string.IsNullOrEmpty(warehouseCode))
                                                     && d.SettleDate.ToString("yyyy-MM-dd") == settleDate)
                                           .ToArray();
                    DailyBalanceRepository.Delete(oldDailyBalance);
                    DailyBalanceRepository.SaveChanges();

                    var query = inQuery.Where(a => (a.InBillMaster.WarehouseCode == warehouseCode
                                                     || string.IsNullOrEmpty(warehouseCode))
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
                    }).Union(outQuery.Where(a => (a.OutBillMaster.WarehouseCode == warehouseCode
                                                   || string.IsNullOrEmpty(warehouseCode))
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
                    })).Union(profitLossQuery.Where(a => (a.ProfitLossBillMaster.WarehouseCode == warehouseCode
                                                           || string.IsNullOrEmpty(warehouseCode))
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
                    })).Union(dailyBalanceQuery.Where(d => (d.WarehouseCode == warehouseCode
                                                             || string.IsNullOrEmpty(warehouseCode))
                                                          && d.SettleDate.ToString("yyyy-MM-dd") == t
                                                          && d.Ending > decimal.Zero
                                              ).Select(a => new
                    {
                        BillDate = settleDate,
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
                        DeliveryAmount = a.Sum(d => d.DeliveryAmount),
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
                    scope.Complete();
                }
                return true;
            }
            catch (Exception e)
            {
                errorInfo = "日结时出现错误，详情：" + e.Message;
                return false;
            }
        }
    }
}
