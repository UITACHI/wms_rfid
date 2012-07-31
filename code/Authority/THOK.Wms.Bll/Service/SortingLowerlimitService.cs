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
    public class SortingLowerlimitService : ServiceBase<SortingLowerlimit>, ISortingLowerlimitService
    {
        [Dependency]
        public ISortingLowerlimitRepository SortingLowerlimitRepository { get; set; }

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortingLowerlimitService 成员

        public object GetDetails(int page, int rows, string sortingLineCode, string sortingLineName, string productName, string productCode, string IsActive)
        {
            IQueryable<SortingLowerlimit> lowerLimitQuery = SortingLowerlimitRepository.GetQueryable();
            var lowerLimit = lowerLimitQuery.Where(s => s.SortingLineCode == s.SortingLineCode);
            if (sortingLineCode != string.Empty && sortingLineCode != null)
            {
                lowerLimit = lowerLimit.Where(l => l.SortingLineCode.Contains(sortingLineCode));
            }
            if (sortingLineName != string.Empty && sortingLineName != null)
            {
                lowerLimit = lowerLimit.Where(l => l.SortingLine.SortingLineName.Contains(sortingLineName));
            }
            if (productCode != string.Empty && productCode != null)
            {
                lowerLimit = lowerLimit.Where(l => l.ProductCode.Contains(productCode));
            }
            if (productName != string.Empty && productName != null)
            {
                lowerLimit = lowerLimit.Where(l => l.Product.ProductName.Contains(productName));
            }
            if (IsActive != string.Empty && IsActive != null)
            {
                lowerLimit = lowerLimit.Where(l => l.IsActive == IsActive);
            }
            var temp = lowerLimit.OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.ID,
                b.SortingLineCode,
                b.SortingLine.SortingLineName,
                b.ProductCode,
                b.Product.ProductName,
                b.UnitCode,
                b.Unit.UnitName,
                Quantity = b.Quantity / b.Unit.Count,
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public new bool Add(SortingLowerlimit sortLowerLimit)
        {
            var lowerLimit = new SortingLowerlimit();
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == sortLowerLimit.UnitCode);
            lowerLimit.SortingLineCode = sortLowerLimit.SortingLineCode;
            lowerLimit.ProductCode = sortLowerLimit.ProductCode;
            lowerLimit.UnitCode = sortLowerLimit.UnitCode;
            lowerLimit.Quantity = sortLowerLimit.Quantity * unit.Count;
            lowerLimit.IsActive = sortLowerLimit.IsActive;
            lowerLimit.UpdateTime = DateTime.Now;

            SortingLowerlimitRepository.Add(lowerLimit);
            SortingLowerlimitRepository.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            int ID = Convert.ToInt32(id);
            var lowerLimit = SortingLowerlimitRepository.GetQueryable()
               .FirstOrDefault(s => s.ID == ID);
            if (lowerLimit != null)
            {
                SortingLowerlimitRepository.Delete(lowerLimit);
                SortingLowerlimitRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(SortingLowerlimit sortLowerLimit)
        {
            var lowerLimitSave = SortingLowerlimitRepository.GetQueryable().FirstOrDefault(s => s.ID == sortLowerLimit.ID);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == sortLowerLimit.UnitCode);
            lowerLimitSave.SortingLineCode = sortLowerLimit.SortingLineCode;
            lowerLimitSave.ProductCode = sortLowerLimit.ProductCode;
            lowerLimitSave.UnitCode = sortLowerLimit.UnitCode;
            lowerLimitSave.Quantity = sortLowerLimit.Quantity * unit.Count;
            lowerLimitSave.IsActive = sortLowerLimit.IsActive;
            lowerLimitSave.UpdateTime = DateTime.Now;

            SortingLowerlimitRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
