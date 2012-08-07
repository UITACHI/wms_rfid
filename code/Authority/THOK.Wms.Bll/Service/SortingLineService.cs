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
            var sortLine = sortLineQuery.Where(s => s.SortingLineCode == s.SortingLineCode);
            if (sortingLineCode != string.Empty && sortingLineCode != null)
            {
                sortLine = sortLine.Where(s => s.SortingLineCode.Contains(sortingLineCode));
            }
            if (sortingLineName != string.Empty && sortingLineName != null)
            {
                sortLine = sortLine.Where(s => s.SortingLineName.Contains(sortingLineName));
            }
            if (SortingLineType != string.Empty && SortingLineType != null)
            {
                sortLine = sortLine.Where(s => s.SortingLineType.Contains(SortingLineType));
            }
            if (IsActive != string.Empty && IsActive != null)
            {
                sortLine = sortLine.Where(s => s.IsActive == IsActive);
            }

            var temp = sortLine.OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.SortingLineCode,
                b.SortingLineName,
                SortingLineType = b.SortingLineType == "1" ? "半自动分拣线" : "全自动分拣线",
                b.OutBillTypeCode,
                b.MoveBillTypeCode,
                CellName = b.Cell != null ? b.Cell.CellName : "",
                b.CellCode,
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public new bool Add(SortingLine sortingLine)
        {
            var sortLine = new SortingLine();
            sortLine.SortingLineCode = sortingLine.SortingLineCode;
            sortLine.SortingLineName = sortingLine.SortingLineName;
            sortLine.SortingLineType = sortingLine.SortingLineType;
            sortLine.OutBillTypeCode = sortingLine.OutBillTypeCode;
            sortLine.MoveBillTypeCode = sortingLine.MoveBillTypeCode;
            sortLine.CellCode = sortingLine.CellCode;
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
            sortLineSave.OutBillTypeCode = sortingLine.OutBillTypeCode;
            sortLineSave.MoveBillTypeCode = sortingLine.MoveBillTypeCode;
            sortLineSave.CellCode = sortingLine.CellCode;
            sortLineSave.IsActive = sortingLine.IsActive;
            sortLineSave.UpdateTime = DateTime.Now;

            SortingLineRepository.SaveChanges();
            return true;
        }

        public object GetSortLine()
        {
            var temp = SortingLineRepository.GetQueryable().OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.SortingLineCode,
                b.SortingLineName,
                SortingLineType = b.SortingLineType == "1" ? "半自动分拣线" : "全自动分拣线",
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
            return temp.ToArray();
        }

        #endregion
    }
}
