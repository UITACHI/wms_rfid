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
    public class CurrentStockService : ServiceBase<Storage>, ICurrentStockService
    {
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICurrentStockService 成员

        public object GetCellDetails(int page, int rows, string productCode, string ware, string area, string unitType)
        {
            if (unitType == null || unitType=="")
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
            var storage = storages.GroupBy(s => s.Product.ProductCode)
                 .Select(s => new
                 {
                     ProductCode = s.Max(p => p.Product.ProductCode),
                     ProductName = s.Max(p => p.Product.ProductName),
                     //UnitCode = s.Max(p => p.Product.Unit.UnitCode),
                     //UnitName = s.Max(p => p.Product.Unit.UnitName),
                     Quantity = s.Sum(p => p.Quantity),
                     //UnitCode01 =s.Max(p => p.Product.UnitList.Unit01.UnitCode),
                     UnitName01 =s.Max(p => p.Product.UnitList.Unit01.UnitName),
                     //UnitCode02 =s.Max(p => p.Product.UnitList.Unit02.UnitCode),
                     UnitName02 =s.Max(p => p.Product.UnitList.Unit02.UnitName),
                     Count01 =s.Max(p => p.Product.UnitList.Unit01.Count),
                     Count02 = s.Max(p => p.Product.UnitList.Unit02.Count),
                 });
            int total = storage.Count();
            storage = storage.OrderBy(s => s.ProductName);
            storage = storage.Skip((page - 1) * rows).Take(rows);
            if (unitType == "1")
            {
                string unitName1 = "标准件";
                decimal count1 = 10000;
                string unitName2 = "标准条";
                decimal count2 = 200;
                var currentstorage = storage.ToArray().Select(d => new
                {
                    ProductCode = d.ProductCode,
                    ProductName = d.ProductName,
                    UnitName1 = unitName1,
                    UnitName2 = unitName2,
                    Quantity1 = d.Quantity / count1,
                    Quantity2 = d.Quantity / count2,
                    Quantity = d.Quantity
                });
                return new { total, rows = currentstorage.ToArray() };
            }
            if (unitType == "2")
            {
                var currentstorage = storage.ToArray().Select(d => new
                {
                    ProductCode = d.ProductCode,
                    ProductName = d.ProductName,
                    UnitName1 = d.UnitName01,
                    UnitName2 = d.UnitName02,
                    Quantity1 = d.Quantity / d.Count01,
                    Quantity2 = d.Quantity / d.Count02,
                    Quantity = d.Quantity
                });
                return new { total, rows = currentstorage.ToArray() };
            }
            return new { total, rows = storage.ToArray() };
        }

        #endregion
    }
}
