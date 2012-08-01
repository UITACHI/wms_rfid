using System;
using System.Linq;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class StockledgerService : IStockledgerService
    {
        [Dependency]
        public IDailyBalanceRepository StockledgerRepository { get; set; }

        //protected override Type LogPrefix
        //{
        //    get { return this.GetType(); }
        //}

        #region IStockledgerService 成员

        public object GetDetails(int page, int rows, string warehouseCode, string productCode, string beginDate, string endDate)
        {
            var ledgerQuery = StockledgerRepository.GetQueryable().AsEnumerable();
            var query = ledgerQuery.Where(i => i.ProductCode.Contains(productCode)
                                         && i.WarehouseCode.Contains(warehouseCode)
                                       ).OrderBy(i => i.SettleDate).OrderBy(i => i.Product.ProductName
                                       ).AsEnumerable().Select(i => new
                                       {
                                           SettleDate = i.SettleDate.ToString("yyyy-MM-dd"),
                                           i.WarehouseCode,
                                           i.Warehouse.WarehouseName,
                                           i.ProductCode,
                                           i.Product.ProductName,
                                           i.UnitCode,
                                           i.Unit.UnitName,
                                           i.Beginning,
                                           Item_Beginning=Convert.ToDouble(i.Beginning/50),
                                           i.EntryAmount,
                                           Item_EntryAmount = Convert.ToDouble(i.EntryAmount / 50),
                                           i.DeliveryAmount,
                                           Item_DeliveryAmount = Convert.ToDouble(i.DeliveryAmount / 50),
                                           i.ProfitAmount,
                                           i.LossAmount,
                                           ProfitLossAmount = i.ProfitAmount > 0 ? i.ProfitAmount : 0-i.LossAmount,
                                           Item_ProfitLossAmount = Convert.ToDouble(i.ProfitAmount > 0 ? (i.ProfitAmount / 50) : 0 - (i.LossAmount / 50)),
                                           i.Ending
            });
            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                query = query.Where(i => Convert.ToDateTime(i.SettleDate) >= begin);
            }

            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate);
                query = query.Where(i => Convert.ToDateTime(i.SettleDate) <= end);
            }
            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = query.ToArray() };
        }

        #endregion
    }
}
