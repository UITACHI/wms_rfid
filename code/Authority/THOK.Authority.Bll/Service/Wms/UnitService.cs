using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Bll.Interfaces.Wms;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces.Wms;

namespace THOK.Authority.Bll.Service.Wms
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
            var unit = unitQuery.Where(u => u.UnitCode.Contains(UnitCode) && u.UnitName.Contains(UnitName)).OrderBy(u => u.UnitCode).AsEnumerable().Select(u => new { u.UnitCode, u.UnitName, u.COUNT, IsActive = u.IsActive == "1" ? "可用" : "不可用", UpdateTime = u.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                unit = unitQuery.Where(u => u.UnitCode.Contains(UnitCode) && u.UnitName.Contains(UnitName) && u.IsActive.Contains(IsActive)).OrderBy(u => u.UnitCode).AsEnumerable().Select(u => new { u.UnitCode, u.UnitName, u.COUNT, IsActive = u.IsActive == "1" ? "可用" : "不可用", UpdateTime = u.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            }
            int total = unit.Count();
            unit = unit.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = unit.ToArray() };
        }

        public new bool Add(Unit unit)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string UnitCode)
        {
            throw new NotImplementedException();
        }

        public bool Save(Unit unit)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
