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
    public class SortingLineService : ServiceBase<SortingLine>, ISortingLineService
    {
        [Dependency]
        public ISortingLineRepository SortingLineRepository { get; set; }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortingLineService 成员

        public object GetDetails(int page, int rows, string sortingLineCode, string sortingLineName, string SortingLineType, string IsActive)
        {
            IQueryable<SortingLine> sortLineQuery = SortingLineRepository.GetQueryable();
            var sortLine = sortLineQuery.Where(s => s.SortingLineCode.Contains(sortingLineCode) || s.SortingLineName.Contains(sortingLineName)|| s.SortingLineType.Contains(SortingLineType) || s.IsActive.Contains(IsActive)).OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.SortingLineCode,
                b.SortingLineName,
                SortingLineType=b.SortingLineType=="1"?"半自动分拣线":"全自动分拣线",
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = sortLine.Count();
            sortLine = sortLine.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = sortLine.ToArray() };
        }

        public new bool Add(SortingLine sortingLine)
        {
            var sortLine = new SortingLine();
            sortLine.SortingLineCode = sortingLine.SortingLineCode;
            sortLine.SortingLineName = sortingLine.SortingLineName;
            sortLine.SortingLineType = sortingLine.SortingLineType;
            sortLine.IsActive = sortingLine.IsActive;
            sortLine.UpdateTime = DateTime.Now;

            SortingLineRepository.Add(sortLine);
            SortingLineRepository.SaveChanges();
            return true;
        }

        public bool Delete(string sortingLineCode)
        {
            var sortLine = SortingLineRepository.GetQueryable()
                .FirstOrDefault(s => s.SortingLineCode == sortingLineCode);
            if (sortLine != null)
            {
                SortingLineRepository.Delete(sortLine);
                SortingLineRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(SortingLine sortingLine)
        {
            var sortLineSave = SortingLineRepository.GetQueryable().FirstOrDefault(s => s.SortingLineCode == sortingLine.SortingLineCode);
            sortLineSave.SortingLineName = sortingLine.SortingLineName;
            sortLineSave.SortingLineType = sortingLine.SortingLineType;
            sortLineSave.IsActive = sortingLine.IsActive;
            sortLineSave.UpdateTime = DateTime.Now;

            SortingLineRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
