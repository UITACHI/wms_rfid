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
    public class UnitListService:ServiceBase<UnitList>,IUnitListService
    {
        [Dependency]
        public IUnitListRepository UnitListRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IUnitListService 成员

        public object GetDetails(int page, int rows, UnitList uls)
        {
            
            IQueryable<UnitList> unitListQuery = UnitListRepository.GetQueryable();
            var unitList = unitListQuery.Where(ul =>
                ul.UnitListCode.Contains(uls.UnitListCode)
                && ul.UnitListName.Contains(uls.UnitListName)
                && ul.UniformCode.Contains(uls.UniformCode)
                && ul.UnitCode01.Contains(uls.UnitCode01)
                && ul.UnitName01.Contains(uls.UnitName01)
                && ul.UnitCode02.Contains(uls.UnitCode02)
                && ul.UnitName02.Contains(uls.UnitName02)
                && ul.UnitCode03.Contains(uls.UnitCode03)
                && ul.UnitName03.Contains(uls.UnitName03)
                && ul.UnitCode04.Contains(uls.UnitCode04)
                && ul.UnitName04.Contains(uls.UnitName04)
                && ul.IsActive.Contains(uls.IsActive)
                ).AsEnumerable().Select(ul => new 
            { 
                ul.UnitListCode,
                ul.UniformCode,
                ul.UnitListName,
                Unit01 = ul.UnitName01.ToString() + " ： " + ul.UnitCode01.ToString(),
                Unit02 = ul.UnitName02.ToString() + " ： " + ul.UnitCode02.ToString(),
                Unit03 = ul.UnitName03.ToString() + " ： " + ul.UnitCode03.ToString(),
                Unit04 = ul.UnitName04.ToString() + " ： " + ul.UnitCode04.ToString(),
                ul.UnitCode01,
                ul.UnitName01,
                ul.Quantity01,
                ul.UnitCode02,
                ul.UnitName02,
                ul.Quantity02,
                ul.UnitCode03,
                ul.UnitName03,
                ul.Quantity03,
                ul.UnitCode04,
                ul.UnitName04,
                ul.IsActive,
                UpdateTime = ul.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")

            });
            int total = unitList.Count();
            unitList = unitList.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = unitList.ToArray() };
        }

        public new bool Add(UnitList unitlist)
        {
            var ul = new UnitList();
            ul.UnitListCode = unitlist.UnitListCode;
            ul.UniformCode = unitlist.UniformCode;
            ul.UnitListName = unitlist.UnitListCode;
            ul.UnitCode01 = unitlist.UnitCode01;
            ul.UnitName01 = unitlist.UnitName01;
            ul.Quantity01 = unitlist.Quantity01;
            ul.UnitCode02 = unitlist.UnitCode02;
            ul.UnitName02 = unitlist.UnitName02;
            ul.Quantity02 = unitlist.Quantity02;
            ul.UnitCode03 = unitlist.UnitCode03;
            ul.UnitName03 = unitlist.UnitName03;
            ul.Quantity03 = unitlist.Quantity03;
            ul.UnitCode04 = unitlist.UnitCode04;
            ul.UnitName04 = unitlist.UnitName04;
            ul.IsActive = unitlist.IsActive;
            ul.UpdateTime = DateTime.Now;
            UnitListRepository.Add(ul);
            UnitListRepository.SaveChanges();
            return true;
        }

        public bool Delete(string unitlistCode)
        {
            var unitlist = UnitListRepository.GetQueryable()
                .FirstOrDefault(b => b.UnitListCode == unitlistCode);
            if (unitlistCode != null)
            {
                UnitListRepository.Delete(unitlist);
                UnitListRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(UnitList unitlist)
        {
            var ul = UnitListRepository.GetQueryable().FirstOrDefault(u => u.UnitListCode == unitlist.UnitListCode);
            ul.UnitListCode = unitlist.UnitListCode;
            ul.UniformCode = unitlist.UniformCode;
            ul.UnitListName = unitlist.UnitListCode;
            ul.UnitCode01 = unitlist.UnitCode01;
            ul.UnitName01 = unitlist.UnitName01;
            ul.Quantity01 = unitlist.Quantity01;
            ul.UnitCode02 = unitlist.UnitCode02;
            ul.UnitName02 = unitlist.UnitName02;
            ul.Quantity02 = unitlist.Quantity02;
            ul.UnitCode03 = unitlist.UnitCode03;
            ul.UnitName03 = unitlist.UnitName03;
            ul.Quantity03 = unitlist.Quantity03;
            ul.UnitCode04 = unitlist.UnitCode04;
            ul.UnitName04 = unitlist.UnitName04;
            ul.IsActive = unitlist.IsActive;
            ul.UpdateTime = DateTime.Now;
            UnitListRepository.SaveChanges();
            return true;
        }
        #endregion
    }
}
