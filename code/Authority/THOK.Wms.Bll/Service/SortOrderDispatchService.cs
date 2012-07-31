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
    public class SortOrderDispatchService : ServiceBase<SortOrderDispatch>, ISortOrderDispatchService
    {
        [Dependency]
        public ISortOrderDispatchRepository SortOrderDispatchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortOrderDispatchService 成员

        public object GetDetails(int page, int rows, string OrderDate, string SortingLineCode, string DeliverLineCode)
        {
            IQueryable<SortOrderDispatch> sortDispatchQuery = SortOrderDispatchRepository.GetQueryable();
            var sortDispatch = sortDispatchQuery.Where(s => s.SortingLineCode.Contains(SortingLineCode) && s.DeliverLineCode.Contains(DeliverLineCode) && s.OrderDate.Contains(OrderDate)).OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.SortingLineCode,
                b.SortingLine.SortingLineName,
                b.OrderDate,
                b.DeliverLineCode,
                b.DeliverLine.DeliverLineName,
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = sortDispatch.Count();
            sortDispatch = sortDispatch.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = sortDispatch.ToArray() };
        }

        public new bool Add(SortOrderDispatch sortDispatch)
        {
            var sortOrderDispatch = new SortOrderDispatch();
            sortOrderDispatch.SortingLineCode = sortDispatch.SortingLineCode;
            sortOrderDispatch.DeliverLineCode = sortDispatch.DeliverLineCode;
            sortOrderDispatch.OrderDate = sortDispatch.OrderDate;
            sortOrderDispatch.IsActive = sortDispatch.IsActive;
            sortOrderDispatch.UpdateTime = DateTime.Now;

            SortOrderDispatchRepository.Add(sortOrderDispatch);
            SortOrderDispatchRepository.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            int ID = Convert.ToInt32(id);
            var sortOrderDispatch = SortOrderDispatchRepository.GetQueryable()
               .FirstOrDefault(s => s.ID == ID);
            if (sortOrderDispatch != null)
            {
                SortOrderDispatchRepository.Delete(sortOrderDispatch);
                SortOrderDispatchRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(SortOrderDispatch sortDispatch)
        {
            var sortOrderDispatch = SortOrderDispatchRepository.GetQueryable().FirstOrDefault(s => s.ID == sortDispatch.ID);
            sortOrderDispatch.SortingLineCode = sortDispatch.SortingLineCode;
            sortOrderDispatch.DeliverLineCode = sortDispatch.DeliverLineCode;
            sortOrderDispatch.OrderDate = sortDispatch.OrderDate;
            sortOrderDispatch.IsActive = sortDispatch.IsActive;
            sortOrderDispatch.UpdateTime = DateTime.Now;

            SortOrderDispatchRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
