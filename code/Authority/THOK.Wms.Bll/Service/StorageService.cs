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
    public class StorageService : ServiceBase<Storage>, IStorageService
    {
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IStorageService 成员

        public object GetDetails(int page, int rows, string type, string id)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.OrderBy(s => s.StorageCode).AsEnumerable().Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            if (type == "ware")
            {
                storages = storageQuery.Where(s => s.cell.Shelf.Area.Warehouse.WarehouseCode == id)
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            }
            else if (type == "area")
            {
                storages = storageQuery.Where(s => s.cell.Shelf.Area.AreaCode == id)
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            }
            else if (type == "shelf")
            {
                storages = storageQuery.Where(s => s.cell.Shelf.ShelfCode == id)
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            }
            else if (type == "cell")
            {
                storages = storageQuery.Where(s => s.cell.CellCode == id)
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            }
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storages.ToArray() };
        }


        public object GetDetails(int page, int rows, string infoStr)
        {
            string[] infos = infoStr.Split(',');
            string type;
            string id;
            for (int i = 0; i < infos.Length; i++)
            {
                string[] types = infos[i].Split('^');
                type = types[0];
                id = types[1];
            }
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.OrderBy(s => s.StorageCode).AsEnumerable().Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //if (type == "ware")
            //{
            //    storages = storageQuery.Where(s => s.cell.Shelf.Area.Warehouse.WarehouseCode == id)
            //                           .OrderBy(s => s.StorageCode).AsEnumerable()
            //                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //}
            //else if (type == "area")
            //{
            //    storages = storageQuery.Where(s => s.cell.Shelf.Area.AreaCode == id)
            //                           .OrderBy(s => s.StorageCode).AsEnumerable()
            //                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //}
            //else if (type == "shelf")
            //{
            //    storages = storageQuery.Where(s => s.cell.Shelf.ShelfCode == id)
            //                           .OrderBy(s => s.StorageCode).AsEnumerable()
            //                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //}
            //else if (type == "cell")
            //{
            //    storages = storageQuery.Where(s => s.cell.CellCode == id)
            //                           .OrderBy(s => s.StorageCode).AsEnumerable()
            //                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //}
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storages.ToArray() };
        }

        #endregion
    }
}
