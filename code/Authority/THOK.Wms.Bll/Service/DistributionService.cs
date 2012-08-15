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
    public class DistributionService : ServiceBase<Storage>, IDistributionService
    {
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IDistributionService 成员

        public object GetCellDetails(int page, int rows, string productCode, string ware, string area, string unitType)
        {
            if (unitType == null || unitType == "")
            {
                unitType = "1";
            }

            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.Where(s => s.Quantity > 0 && s.IsLock == "0");
            if (ware != null && ware != string.Empty || area != null && area != string.Empty)
            {
                if (ware != string.Empty)
                {
                    ware = ware.Substring(0, ware.Length - 1);
                }
                if (area != string.Empty)
                {
                    area = area.Substring(0, area.Length - 1);
                }

                storages = storages.Where(s => (ware.Contains(s.Cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.Cell.Shelf.Area.AreaCode)));
            }
            if (productCode != string.Empty)
            {
                storages = storages.Where(s => s.ProductCode == productCode);
            }
            storages = storages.OrderBy(s => s.Product.ProductName);
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            if (unitType == "1")
            {
                string unitName1 = "标准件";
                decimal count1 = 10000;
                string unitName2 = "标准条";
                decimal count2 = 200;
                var currentstorage = storages.ToArray().Select(d => new
                {
                    ProductCode = d.ProductCode,
                    ProductName = d.Product.ProductName,
                    d.Cell.CellCode,
                    d.Cell.CellName,
                    UnitName1 = unitName1,
                    UnitName2 = unitName2,
                    Quantity1 = d.Quantity / count1,
                    Quantity2 = d.Quantity / count2,
                    Quantity = d.Quantity,
                    StorageTime=d.StorageTime.ToString("yyyy-MM-dd")
                });
                return new { total, rows = currentstorage.ToArray() };
            }
            if (unitType == "2")
            {
                var currentstorage = storages.ToArray().Select(d => new
                {
                    ProductCode = d.ProductCode,
                    ProductName = d.Product.ProductName,
                    d.Cell.CellCode,
                    d.Cell.CellName,
                    UnitName1 = d.Product.UnitList.Unit01.UnitName,
                    UnitName2 = d.Product.UnitList.Unit02.UnitName,
                    Quantity1 = d.Quantity / d.Product.UnitList.Unit01.Count,
                    Quantity2 = d.Quantity / d.Product.UnitList.Unit02.Count,
                    Quantity = d.Quantity,
                    StorageTime=d.StorageTime.ToString("yyyy-MM-dd")
                });
                return new { total, rows = currentstorage.ToArray() };
            }
            return new { total, rows = storages.ToArray() };
        }

        #endregion
    }
}
