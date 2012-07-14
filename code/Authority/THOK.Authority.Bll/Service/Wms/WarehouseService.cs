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
    public class WarehouseService : ServiceBase<Warehouse>, IWarehouseService
    {
        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IWarehouseService 成员

        public object GetDetails(int page, int rows, string warehouseCode)
        {
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            var warehouse = wareQuery.OrderBy(b => b.WarehouseCode).AsEnumerable().Select(b => new { b.WarehouseCode, b.WarehouseName, b.WarehouseType, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            int total = warehouse.Count();
            warehouse = warehouse.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = warehouse.ToArray() };
        }

        public new bool Add(Warehouse warehouse)
        {
            var ware = new Warehouse();
            ware.WarehouseCode = warehouse.WarehouseCode;
            ware.WarehouseName = warehouse.WarehouseName;
            ware.WarehouseType = warehouse.WarehouseType;
            ware.ShortName = warehouse.ShortName;
            ware.CompanyCode = "";// warehouse.CompanyCode;
            ware.Description = warehouse.Description;
            ware.IsActive = warehouse.IsActive;
            ware.UpdateTime = DateTime.Now;

            WarehouseRepository.Add(ware);
            WarehouseRepository.SaveChanges();
            return true;
        }

        public bool Delete(string warehouseCode)
        {

            var warehouse = WarehouseRepository.GetQueryable()
                .FirstOrDefault(w => w.WarehouseCode == warehouseCode);
            if (warehouse != null)
            {
                WarehouseRepository.Delete(warehouse);
                WarehouseRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Warehouse warehouse)
        {
            var ware = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == warehouse.WarehouseCode);
            ware.WarehouseCode = warehouse.WarehouseCode;
            ware.WarehouseName = warehouse.WarehouseName;
            ware.WarehouseType = warehouse.WarehouseType;
            ware.ShortName = warehouse.ShortName;
            ware.CompanyCode = warehouse.CompanyCode;
            ware.Description = warehouse.Description;
            ware.IsActive = warehouse.IsActive;
            ware.UpdateTime = DateTime.Now;

            WarehouseRepository.SaveChanges();
            return true;
        }

        #endregion
    }
}
