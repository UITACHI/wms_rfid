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
        public object GetDetails(int page, int rows)
        {
            IQueryable<DailyBalance> dailyBalanceQuery = DailyBalanceRepository.GetQueryable();
            var dailyBalance = dailyBalanceQuery.OrderBy(c => c.SettleDate).AsEnumerable()
                                                .GroupBy(c => c.SettleDate)
                                                .Select(c => new { SettleDate=c.Key.ToString("yyyy-MM-dd"),
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
        public object MXGetDetails(int page, int rows, string date)
        {
            IQueryable<DailyBalance> mxdailyBalanceQuery = DailyBalanceRepository.GetQueryable();
            DateTime dt = DateTime.Parse(date);
            var mxdailyBalance = mxdailyBalanceQuery.Where(b => dt.Equals(b.SettleDate))
                .OrderBy(b => b.ID).AsEnumerable()
                .Select(b => new { b.ID, SettleDate = b.SettleDate.ToString("yyyy-MM-dd HH:mm:ss"), b.WarehouseCode, b.ProductCode, b.UnitCode, b.Beginning, b.EntryAmount, b.DeliveryAmount, b.ProfitAmount, b.LossAmount, b.Ending });
            int total = mxdailyBalance.Count();
            mxdailyBalance = mxdailyBalance.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = mxdailyBalance.ToArray() };
        }
    }
}
