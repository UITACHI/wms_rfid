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
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable().Where(p => p.Quantity > 0);
            var tmp = storageQuery.GroupBy(s => new
                                                {
                                                    s.ProductCode,
                                                    s.Product.ProductName,
                                                    s.Cell.WarehouseCode,
                                                    s.Cell.Warehouse.WarehouseName,
                                                    s.Cell.AreaCode,
                                                    s.Cell.Area.AreaName,
                                                    s.Cell.ShelfCode,
                                                    s.Cell.Shelf.ShelfName
                                                }
                                   )
                                   .Select(s => new
                                   {
                                       s.Key.ProductCode,
                                       s.Key.ProductName,
                                       s.Key.WarehouseCode,
                                       s.Key.WarehouseName,
                                       s.Key.AreaCode,
                                       s.Key.AreaName,
                                       s.Key.ShelfCode,
                                       s.Key.ShelfName,
                                       Quantity = s.Sum(p => p.Quantity)
                                   })
                                   .GroupBy(s => new
                                   {
                                       s.ProductCode,
                                       s.ProductName
                                   })
                                   .Select(s => new
                                   {
                                       s.Key.ProductCode,
                                       s.Key.ProductName,
                                       Quantity = s.Sum(p => p.Quantity),
                                       WareHouses = s.GroupBy(w => new
                                       {
                                           w.WarehouseCode,
                                           w.WarehouseName
                                       })
                                       .Select(w => new
                                       {
                                           w.Key.WarehouseCode,
                                           w.Key.WarehouseName,
                                           Quantity = w.Sum(q => q.Quantity),
                                           Areas = w.GroupBy(a => new
                                           {
                                               a.AreaCode,
                                               a.AreaName
                                           })
                                           .Select(a => new
                                           {
                                               a.Key.AreaCode,
                                               a.Key.AreaName,
                                               Quantity = a.Sum(q => q.Quantity),
                                               Shelfs = a.GroupBy(sh => new
                                               {
                                                   sh.ShelfCode,
                                                   sh.ShelfName
                                               })
                                               .Select(sh => new
                                               {
                                                   sh.Key.ShelfCode,
                                                   sh.Key.ShelfName,
                                                   Quantity = sh.Sum(q => q.Quantity)
                                               })
                                           })
                                       })
                                   }).ToArray();

            HashSet<Tree> productSet = new HashSet<Tree>();
            foreach (var product in tmp)
            {
                Tree productTree = new Tree();
                productTree.id = product.ProductCode;
                productTree.text = product.ProductName + "(  数量：" + product.Quantity + " )";
                productTree.state = "open";
                productTree.attributes = "product";

                HashSet<Tree> warehouseSet = new HashSet<Tree>();
                foreach (var wareHouse in product.WareHouses)
                {
                    Tree warehouseTree = new Tree();
                    warehouseTree.id = wareHouse.WarehouseCode;
                    warehouseTree.text = "仓库：" + wareHouse.WarehouseName + "(  数量：" + wareHouse.Quantity + " )";
                    warehouseTree.state = "open";
                    warehouseTree.attributes = "warehouse";

                    HashSet<Tree> areaSet = new HashSet<Tree>();
                    foreach (var area in wareHouse.Areas)
                    {
                        Tree areaTree = new Tree();
                        areaTree.id = area.AreaCode;
                        areaTree.text = "库区：" + area.AreaName + "(  数量：" + area.Quantity + " )";
                        areaTree.state = "close";
                        areaTree.attributes = "area";

                        HashSet<Tree> shelfSet = new HashSet<Tree>();
                        foreach (var shelf in area.Shelfs)
                        {
                            Tree shelfTree = new Tree();
                            shelfTree.id = shelf.ShelfCode;
                            shelfTree.text = "货架：" + shelf.ShelfName + "(  数量：" + shelf.Quantity + " )";
                            shelfTree.state = "close";
                            shelfTree.attributes = "shelf";
                            shelfSet.Add(shelfTree);
                        }
                        areaTree.children = shelfSet.ToArray();
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
    }
}
