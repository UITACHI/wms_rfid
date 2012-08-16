using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;

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

        public object GetProductTree()
        {
            IQueryable<Storage> products = StorageRepository.GetQueryable().Where(p => p.Quantity > 0 && p.IsLock == "0")
                                                                           .OrderBy(p => p.Product.ProductName);
            var productRepository = products.ToArray().OrderBy(p => p.Product.ProductName).GroupBy(s => s.Product.ProductCode)
                                                                           .Select(s => new
                                                                           {
                                                                               ProductCode = s.Max(p => p.Product.ProductCode),
                                                                               ProductName = s.Max(p => p.Product.ProductName),
                                                                               Quantity = s.Sum(p => p.Quantity)
                                                                           });
            HashSet<Tree> productSet = new HashSet<Tree>();
            int aaa = productRepository.Count();
            foreach (var product in productRepository)
            {
                Tree productTree = new Tree();
                productTree.id = product.ProductCode;
                productTree.text =product.ProductName+ "(  数量："+ product.Quantity +" )";
                productTree.state = "open";
                productTree.attributes = "product";

                var warehouses = products.Where(w => w.ProductCode == product.ProductCode)
                                         .OrderBy(w => w.Cell.Area.WarehouseCode)
                                         .GroupBy(w => w.Cell.Area.WarehouseCode)
                                         .Select(w => new
                                         {
                                             WarehouseCode = w.Max(p => p.Cell.Area.WarehouseCode),
                                             WarehouseName = w.Max(p => p.Cell.Area.Warehouse.WarehouseName),
                                             Quantity = w.Sum(p => p.Quantity)
                                         });
                HashSet<Tree> warehouseSet = new HashSet<Tree>();
                int bbb = warehouses.Count();
                foreach (var warehouse in warehouses)
                {
                    Tree warehouseTree = new Tree();
                    warehouseTree.id = warehouse.WarehouseCode;
                    warehouseTree.text = "仓库：" + warehouse.WarehouseName + "(  数量：" + warehouse.Quantity + " )";
                    warehouseTree.state = "open";
                    warehouseTree.attributes = "warehouse";
                    var areas = products.Where(a => a.Cell.Area.WarehouseCode == warehouse.WarehouseCode &&  a.ProductCode == product.ProductCode)
                                          .OrderBy(a => a.Cell.AreaCode)
                                          .GroupBy(a => a.Cell.AreaCode)
                                          .Select(a => new
                                          {
                                              AreaCode = a.Max(p => p.Cell.AreaCode),
                                              AreaName = a.Max(p => p.Cell.Area.AreaName),
                                              Quantity = a.Sum(p => p.Quantity)
                                          });
                    HashSet<Tree> areaSet = new HashSet<Tree>();
                    int ccc = areas.Count();
                    foreach (var area in areas)
                    {
                        Tree areaTree = new Tree();
                        areaTree.id = area.AreaCode;
                        areaTree.text = "库区：" + area.AreaName + "(  数量：" + area.Quantity + " )";
                        areaTree.state = "close";
                        areaTree.attributes = "area";
                        //var shelfs = warehouses.Where(s => s.Cell.AreaCode == area.Cell.AreaCode)
                        //                       .OrderBy(s => s.Cell.ShelfCode).Select(s => s);
                        //HashSet<Tree> shelfSet = new HashSet<Tree>();
                        //foreach (var shelf in shelfs)
                        //{
                        //    Tree shelfTree = new Tree();
                        //    shelfTree.id = shelf.Cell.ShelfCode;
                        //    shelfTree.text = "货架：" + shelf.Cell.Shelf.ShelfName;
                        //    shelfTree.state = "close";
                        //    shelfTree.attributes = "shelf";
                        //    shelfSet.Add(shelfTree);
                        //}
                        //areaTree.children = shelfSet.ToArray();
                        areaSet.Add(areaTree);
                    }
                    warehouseTree.children = areaSet.ToArray();
                    warehouseSet.Add(warehouseTree);
                }
                productTree.children = warehouseSet.ToArray();
                productSet.Add(productTree);
            }
            return productSet.ToArray();
        }

        #endregion

    }
}
