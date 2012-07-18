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
    public class AreaService : ServiceBase<Area>, IAreaService
    {
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IAreaService 成员

        public object GetDetails(string warehouseCode, string areaCode)
        {
            IQueryable<Area> areaQuery = AreaRepository.GetQueryable();
            var area = areaQuery.OrderBy(b => b.AreaCode).AsEnumerable().Select(b=> new { b.AreaCode, b.AreaName, b.AreaType,b.ShortName,b.AllotInOrder,b.AllotOutOrder,b.Description,b.Warehouse.WarehouseCode,b.Warehouse.WarehouseName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (warehouseCode != null && warehouseCode != string.Empty){
                area = area.Where(a => a.WarehouseCode == warehouseCode).OrderBy(a => a.AreaCode).Select(a => a);
            }
            if (areaCode != null && areaCode!=string.Empty){
                area = area.Where(a => a.AreaCode == areaCode).OrderBy(a => a.AreaCode).Select(a => a);
            }           
            return area.ToArray();
        }

        public new bool Add(Area area)
        {
            var areaAdd = new Area();
            var warehouse = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == area.WarehouseCode);
            areaAdd.AreaCode = area.AreaCode;
            areaAdd.AreaName = area.AreaName;
            areaAdd.ShortName = area.ShortName;
            areaAdd.AreaType = area.AreaType;
            areaAdd.AllotInOrder = area.AllotInOrder;
            areaAdd.AllotOutOrder = area.AllotOutOrder;
            areaAdd.Warehouse = warehouse;
            areaAdd.Description = area.Description;
            areaAdd.IsActive = area.IsActive;
            areaAdd.UpdateTime = DateTime.Now;

            AreaRepository.Add(areaAdd);
            AreaRepository.SaveChanges();
            return true;
        }

        public bool Delete(string areaCode)
        {
            var area = AreaRepository.GetQueryable()
                .FirstOrDefault(a => a.AreaCode == areaCode);
            if (area != null)
            {
                AreaRepository.Delete(area);
                AreaRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Area area)
        {
            var areaSave = AreaRepository.GetQueryable().FirstOrDefault(a => a.AreaCode == area.AreaCode);
            var warehouse = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == area.WarehouseCode);
            areaSave.AreaCode = areaSave.AreaCode;
            areaSave.AreaName = area.AreaName;
            areaSave.ShortName = area.ShortName;
            areaSave.AreaType = area.AreaType;
            areaSave.AllotInOrder = area.AllotInOrder;
            areaSave.AllotOutOrder = area.AllotOutOrder;
            areaSave.Warehouse = warehouse;
            areaSave.Description = area.Description;
            areaSave.IsActive = area.IsActive;
            areaSave.UpdateTime = DateTime.Now;

            AreaRepository.SaveChanges();
            return true;
        }

        public object FindArea(string parameter)
        {
            IQueryable<Area> areaQuery = AreaRepository.GetQueryable();
            var area = areaQuery.Where(a => a.AreaCode == parameter).OrderBy(b => b.AreaCode).AsEnumerable().Select(b => new { b.AreaCode, b.AreaName, b.AreaType, b.ShortName,b.AllotInOrder,b.AllotOutOrder, b.Description, b.Warehouse.WarehouseCode, b.Warehouse.WarehouseName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            return area.First(a => a.AreaCode == parameter);
        }

        #endregion
    }
}
