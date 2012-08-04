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

        public object GetInfoDetails(int page, int rows, string settleDate, string warehouseCode)
        {
            IQueryable<DailyBalance> dailyBalanceQuery = DailyBalanceRepository.GetQueryable();
            DateTime date =System.DateTime.Now;
            if (!settleDate.Equals(string.Empty))
            {
                date = Convert.ToDateTime(settleDate);
                
            }
            var dailyBalance = dailyBalanceQuery.Where(c => c.WarehouseCode.Contains(warehouseCode) && Convert.ToDateTime(c.SettleDate) == date)
                                                .OrderBy(c => c.Product.ProductName).AsEnumerable()
                                                .GroupBy(c => c.ProductCode)
                                                .Select(c => new
                                                {
                                                    ProductCode = c.Key,
                                                    ProductName = c.Max(p => p.Product.ProductName),
                                                    WarehouseName = warehouseCode == "" ? "全部仓库" : c.Max(p => p.Warehouse.WarehouseName),
                                                    Beginning = c.Sum(p => p.Beginning),
                                                    EntryAmount = c.Sum(p => p.EntryAmount),
                                                    DeliveryAmount = c.Sum(p => p.DeliveryAmount),
                                                    ProfitAmount = c.Sum(p => p.ProfitAmount),
                                                    LossAmount = c.Sum(p => p.LossAmount),
                                                    Ending = c.Sum(p => p.Ending)
                                                });
            int total = dailyBalance.Count();
            dailyBalance = dailyBalance.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = dailyBalance.ToArray() };
        }

        #endregion
    }
}
