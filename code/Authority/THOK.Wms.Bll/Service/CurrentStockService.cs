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

        public object GetCellDetails(int page, int rows, string productCode, string ware, string area)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.OrderBy(s => s.Product.ProductCode).AsEnumerable().Select(s => new
            {
                s.Product.ProductCode,
                s.Product.ProductName,
                s.Product.Unit.UnitCode,
                s.Product.Unit.UnitName,
                Quantity = s.Quantity / s.Product.Unit.Count,
            });
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

                storages = storageQuery.ToList().Where(s => (ware.Contains(s.Cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.Cell.Shelf.Area.AreaCode)) && s.Quantity > 0 && s.IsLock == "0")
                                       .GroupBy(s => s.Product.ProductCode)
                                       .Select(s => new
                                       {
                                           ProductCode= s.Max(p => p.Product.ProductCode),
                                           ProductName = s.Max(p => p.Product.ProductName),
                                           UnitCode = s.Max(p => p.Product.Unit.UnitCode),
                                           UnitName = s.Max(p => p.Product.Unit.UnitName),
                                           Quantity = s.Sum(p => p.Quantity / p.Product.Unit.Count)
                                       });
            }
            if (productCode != string.Empty)
            {
                storages = storages.Where(s => s.ProductCode == productCode);
            }
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storages.ToArray() };
        }

        #endregion
    }
}
