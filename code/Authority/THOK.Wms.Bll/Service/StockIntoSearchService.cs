using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class StockIntoSearchService :ServiceBase<InBillMaster>,IStockIntoSearchService
    {
        [Dependency]
        public IStockIntoSearchRepository StockIntoSearchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IInBillMasterService 成员

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<InBillMaster> inBillMasterQuery = StockIntoSearchRepository.GetQueryable();
            var inBillMaster = inBillMasterQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { i.BillNo, BillDate = i.BillDate.ToString("yyyy-MM-dd hh:mm:ss"), i.OperatePersonID, Status = i.Status == "1" ? "可用" : "不可用", IsActive = i.IsActive == "1" ? "可用" : "不可用", Description = i.Description, UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                inBillMaster = inBillMaster.Where(i => i.BillNo.Contains(BillNo) && i.IsActive.Contains(IsActive)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { i.BillNo, i.BillDate, i.OperatePersonID, Status = i.Status == "1" ? "可用" : "不可用", IsActive = i.IsActive == "1" ? "可用" : "不可用", Description = i.Description, UpdateTime = i.UpdateTime });
            }
            int total = inBillMaster.Count();
            inBillMaster = inBillMaster.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = inBillMaster.ToArray() };
        }

        #endregion
    }
}
