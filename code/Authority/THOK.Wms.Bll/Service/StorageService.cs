﻿using System;
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

        /// <summary>
        /// 根据类型获和id获取存储表的数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="type">类型</param>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public object GetDetails(int page, int rows, string type, string id)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.OrderBy(s => s.StorageCode).Where(s => s.StorageCode != null);
            if (type == "ware")
            {
                storages = storages.Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == id);
            }
            else if (type == "area")
            {
                storages = storageQuery.Where(s => s.Cell.Shelf.Area.AreaCode == id);
            }
            else if (type == "shelf")
            {
                storages = storageQuery.Where(s => s.Cell.Shelf.ShelfCode == id);
            }
            else if (type == "cell")
            {
                storages = storageQuery.Where(s => s.Cell.CellCode == id);
            }

            var temp = storages.AsEnumerable().Select(s => new
           {
               s.StorageCode,
               s.Cell.CellCode,
               s.Cell.CellName,
               s.Product.ProductCode,
               s.Product.ProductName,
               Quantity = s.Quantity / s.Product.Unit.Count,
               IsActive = s.IsActive == "1" ? "可用" : "不可用",
               StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
               UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
           });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        /// <summary>
        /// 根据参数获取要生成的盘点数据  --货位
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="ware">仓库</param>
        /// <param name="area">库区</param>
        /// <param name="shelf">货架</param>
        /// <param name="cell">货位</param>
        /// <returns></returns>
        public object GetCellDetails(int page, int rows, string ware, string area, string shelf, string cell)
        {
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storages = storageQuery.OrderBy(s => s.StorageCode).AsEnumerable().Select(s => new
            {
                s.StorageCode,
                s.Cell.CellCode,
                s.Cell.CellName,
                s.Product.ProductCode,
                s.Product.ProductName,
                s.Quantity,
                IsActive = s.IsActive == "1" ? "可用" : "不可用",
                StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
            });
            if (ware != null && ware != string.Empty || area != null && area != string.Empty || shelf != null && shelf != string.Empty || cell != null && cell != string.Empty)
            {
                if (ware != string.Empty)
                {
                    ware = ware.Substring(0, ware.Length - 1);
                }
                if (area != string.Empty)
                {
                    area = area.Substring(0, area.Length - 1);
                }
                if (shelf != string.Empty)
                {
                    shelf = shelf.Substring(0, shelf.Length - 1);
                }
                if (cell != string.Empty)
                {
                    cell = cell.Substring(0, cell.Length - 1);
                }

                storages = storageQuery.ToList().Where(s => ware.Contains(s.Cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.Cell.Shelf.Area.AreaCode) || shelf.Contains(s.Cell.Shelf.ShelfCode) || cell.Contains(s.Cell.CellCode))
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new
                                       {
                                           s.StorageCode,
                                           s.Cell.CellCode,
                                           s.Cell.CellName,
                                           s.Product.ProductCode,
                                           s.Product.ProductName,
                                           s.Quantity,
                                           IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                           StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                           UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                       });
            }
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storages.ToArray() };
        }
        
        /// <summary>
        /// 根据参数获取要生成的盘点数据  --产品
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="products">卷烟信息集合</param>
        /// <returns></returns>
        public object GetProductDetails(int page, int rows, string products)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            if (products != string.Empty && products != null)
            {
                products = products.Substring(0, products.Length - 1);

                var storages = storageQuery.ToList().Where(s => products.Contains(s.Product.ProductCode))
                                      .OrderBy(s => s.StorageCode).AsEnumerable()
                                      .Select(s => new
                                      {
                                          s.StorageCode,
                                          s.Cell.CellCode,
                                          s.Cell.CellName,
                                          s.Product.ProductCode,
                                          s.Product.ProductName,
                                          s.Quantity,
                                          IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                          StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                          UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                      });
                int total = storages.Count();
                storages = storages.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = storages.ToArray() };
            }
            return null;
        }

        #endregion
    }
}
