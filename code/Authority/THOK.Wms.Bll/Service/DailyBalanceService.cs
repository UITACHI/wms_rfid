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
                                                .Select(c => new { c.Key,
                                                      ToBeginning = c.Sum(p => p.Beginning) 
                                                });
            int total = dailyBalance.Count();
            dailyBalance = dailyBalance.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = dailyBalance.ToArray() };
        }
    }
}
