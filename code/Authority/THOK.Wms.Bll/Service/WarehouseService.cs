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
    public class WarehouseService : ServiceBase<Warehouse>, IWarehouseService
    {
        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        [Dependency]
        public ICompanyRepository CompanyRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IWarehouseService 成员

        public object GetDetails(int page, int rows, string warehouseCode)
        {
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            var warehouse = wareQuery.OrderBy(b => b.WarehouseCode).AsEnumerable().Select(b => new { b.WarehouseCode, b.WarehouseName, b.WarehouseType, b.Description, b.ShortName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (warehouseCode != null)
            {
                warehouse = warehouse.Where(w => w.WarehouseCode == warehouseCode);
            }
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
            var company = CompanyRepository.GetQueryable().FirstOrDefault(c => c.CompanyCode == warehouse.CompanyCode);
            ware.WarehouseCode = ware.WarehouseCode;
            ware.WarehouseName = warehouse.WarehouseName;
            ware.WarehouseType = warehouse.WarehouseType;
            ware.ShortName = warehouse.ShortName;
            ware.CompanyCode = company != null ? company.CompanyCode : "";
            ware.Description = warehouse.Description;
            ware.IsActive = warehouse.IsActive;
            ware.UpdateTime = DateTime.Now;

            WarehouseRepository.SaveChanges();
            return true;
        }

        public object FindWarehouse(string parameter)
        {
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            var warehouse = wareQuery.Where(w => w.WarehouseCode == parameter).AsEnumerable().Select(w => new { w.WarehouseCode, w.WarehouseName, w.WarehouseType, w.ShortName, w.Description, IsActive = w.IsActive == "1" ? "可用" : "不可用", UpdateTime = w.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            return warehouse.First(w => w.WarehouseCode == parameter);
        }

        #endregion
    }
}
