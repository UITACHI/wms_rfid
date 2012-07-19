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

        [Dependency]
        public ICellRepository CellRepository { get; set; }

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


        public object GetDetails(int page, int rows, string ware, string area, string shelf, string cell)
        {
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            //var cells = cellQuery.OrderBy(c => c.CellCode).Select(s => s);
            var storages = storageQuery.OrderBy(s => s.StorageCode).AsEnumerable().Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            if (ware != null && ware != string.Empty || area != null && area != string.Empty || shelf != null && shelf != string.Empty || cell != null && cell != string.Empty)
            {
                if (ware != string.Empty)
                    ware = ware.Substring(0, ware.Length - 1);
                if (area != string.Empty)
                    area = area.Substring(0, area.Length - 1);
                if (shelf != string.Empty)
                    shelf = shelf.Substring(0, shelf.Length - 1);
                if (cell != string.Empty)
                    cell = cell.Substring(0, cell.Length - 1);

                var cells = cellQuery.Where(c => ware.Contains(c.WarehouseCode))// && area.Contains(c.AreaCode) && shelf.Contains(c.ShelfCode) && cell.Contains(c.CellCode))
                                     .OrderBy(c => c.CellCode)
                                     .Select(c => c.CellCode);

                storages = storageQuery.Where(s => cells.Any(c => c == s.cell.CellCode))
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            }
            //area = area.Substring(0, area.Length - 1);
            //shelf = shelf.Substring(0, shelf.Length - 1);
            //cell = cell.Substring(0, cell.Length - 1);
            //else if (area == "area")
            //{
            //    storages = storageQuery.Where(s => s.cell.Shelf.Area.AreaCode == id)
            //                           .OrderBy(s => s.StorageCode).AsEnumerable()
            //                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //}
            //else 
            //if (shelf !=null && shelf !="")
            //{
            //    storages = storageQuery.Where(s =>shelf.Contains(s.cell.Shelf.ShelfCode))
            //                           .OrderBy(s => s.StorageCode).AsEnumerable()
            //                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            //}
            // if (cell !=null && cell!="")
            // {
            //     storages = storageQuery.Where(s =>cell.Contains(s.cell.CellCode))
            //                            .OrderBy(s => s.StorageCode).AsEnumerable()
            //                            .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            // }
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storages.ToArray() };
        }

        #endregion
    }
}
