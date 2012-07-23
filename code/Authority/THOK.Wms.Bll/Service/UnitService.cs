﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public class UnitService:ServiceBase<Unit>,IUnitService
    {
        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IUnitService 成员

        public object GetDetails(int page, int rows, string UnitCode, string UnitName, string IsActive)
        {
            IQueryable<Unit> unitQuery = UnitRepository.GetQueryable();
            var unit = unitQuery.Where(u => u.UnitCode.Contains(UnitCode) && u.UnitName.Contains(UnitName))
                .OrderBy(u => u.UnitCode).AsEnumerable()
                .Select(u => new { u.UnitCode, u.UnitName, COUNT = u.Count, IsActive = u.IsActive == "1" ? "可用" : "不可用", UpdateTime = u.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss"),RowVersion = Convert.ToBase64String(u.RowVersion) });
            if (!IsActive.Equals(""))
            {
                unit = unitQuery.Where(u => u.UnitCode.Contains(UnitCode) && u.UnitName.Contains(UnitName) && u.IsActive.Contains(IsActive))
                    .OrderBy(u => u.UnitCode).AsEnumerable()
                    .Select(u => new { u.UnitCode, u.UnitName, COUNT = u.Count, IsActive = u.IsActive == "1" ? "可用" : "不可用", UpdateTime = u.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss"), RowVersion = Convert.ToBase64String(u.RowVersion) });
            }
            int total = unit.Count();
            unit = unit.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = unit.ToArray() };
        }

        public new bool Add(Unit unit)
        {
            var un = new Unit();
            un.UnitCode = unit.UnitCode;
            un.UnitName = unit.UnitName;
            un.Count = unit.Count;
            un.IsActive = unit.IsActive;
            un.UpdateTime = DateTime.Now;

            UnitRepository.Add(un);
            UnitRepository.SaveChanges();
            return true;
        }

        public bool Delete(string UnitCode)
        {
            var unit = UnitRepository.GetQueryable()
                .FirstOrDefault(b => b.UnitCode == UnitCode);
            if (UnitCode != null)
            {
                UnitRepository.Delete(unit);
                UnitRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Unit unit)
        {
            unit.UpdateTime = DateTime.Now;
            UnitRepository.Attach(unit);
            UnitRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
