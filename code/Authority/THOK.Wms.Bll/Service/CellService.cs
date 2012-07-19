using System;
using System.Collections.Generic;
using System.Linq;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Bll.Models;

namespace THOK.Wms.Bll.Service
{
    public class CellService : ServiceBase<Cell>, ICellService
    {

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        [Dependency]
        public IAreaRepository AreaRepository { get; set; }

        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }

        [Dependency]
        public ICellRepository CellRepository { get; set; }

        [Dependency]
        public IProductRepository ProductRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICellService 成员

        public object GetDetails(int page, int rows, string cellCode)
        {
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            var cell = cellQuery.OrderBy(b => b.CellCode).AsEnumerable().Select(b => new { b.CellCode, b.CellName, b.CellType, b.ShortName, b.Rfid, b.Layer,b.Col,b.ImgX,b.ImgY, b.IsSingle, b.MaxQuantity, b.Description, b.Warehouse.WarehouseName, b.Warehouse.WarehouseCode, b.Area.AreaCode, b.Area.AreaName, b.Shelf.ShelfCode, b.Shelf.ShelfName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            int total = cell.Count();
            cell = cell.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = cell.ToArray() };
        }

        public new bool Add(Cell cell)
        {
            var cellAdd = new Cell();
            var warehouse = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == cell.WarehouseCode);
            var area = AreaRepository.GetQueryable().FirstOrDefault(a => a.AreaCode == cell.AreaCode);
            var shelf = ShelfRepository.GetQueryable().FirstOrDefault(s => s.ShelfCode == cell.ShelfCode);
            var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == cell.DefaultProductCode);
            cellAdd.CellCode = cell.CellCode;
            cellAdd.CellName = cell.CellName;
            cellAdd.ShortName = cell.ShortName;
            cellAdd.CellType = cell.CellType;
            cellAdd.Layer = cell.Layer;
            cellAdd.Col = cell.Col;
            cellAdd.ImgX = cell.ImgX;
            cellAdd.ImgY = cell.ImgY;
            cellAdd.Rfid = cell.Rfid;           
            cellAdd.Warehouse = warehouse;
            cellAdd.Area = area;
            cellAdd.Shelf = shelf;
            cellAdd.Product = product;
            cellAdd.MaxQuantity = cell.MaxQuantity;
            cellAdd.IsSingle = cell.IsSingle;
            cellAdd.Description = cell.Description;
            cellAdd.IsActive = cell.IsActive;
            cellAdd.UpdateTime = DateTime.Now;

            CellRepository.Add(cellAdd);
            CellRepository.SaveChanges();
            return true;
        }

        public bool Delete(string cellCode)
        {
            var cell = CellRepository.GetQueryable()
                .FirstOrDefault(c => c.CellCode == cellCode);
            if (cell != null)
            {
                CellRepository.Delete(cell);
                CellRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Cell cell)
        {
            var cellSave = CellRepository.GetQueryable().FirstOrDefault(c => c.CellCode == cell.CellCode);
            var warehouse = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == cell.WarehouseCode);
            var area = AreaRepository.GetQueryable().FirstOrDefault(a => a.AreaCode == cell.AreaCode);
            var shelf = ShelfRepository.GetQueryable().FirstOrDefault(s => s.ShelfCode == cell.ShelfCode);
            var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == cell.DefaultProductCode);
            cellSave.CellCode = cellSave.CellCode;
            cellSave.CellName = cell.CellName;
            cellSave.ShortName = cell.ShortName;
            cellSave.CellType = cell.CellType;
            cellSave.Layer = cell.Layer;
            cellSave.Col = cell.Col;
            cellSave.ImgX = cell.ImgX;
            cellSave.ImgY = cell.ImgY;
            cellSave.Rfid = cell.Rfid;
            cellSave.Warehouse = warehouse;
            cellSave.Area = area;
            cellSave.Shelf = shelf;
            cellSave.Product = product;
            cellSave.MaxQuantity = cell.MaxQuantity;
            cellSave.IsSingle = cell.IsSingle;
            cellSave.Description = cell.Description;
            cellSave.IsActive = cell.IsActive;
            cellSave.UpdateTime = DateTime.Now;

            CellRepository.SaveChanges();
            return true;
        }

        public object GetWareCheck(string shelfCode)
        {
            var warehouses = WarehouseRepository.GetQueryable().AsEnumerable();          
            HashSet<Tree> wareSet = new HashSet<Tree>();
            if (shelfCode == null || shelfCode == string.Empty)//判断是否是加载货位
            {
                foreach (var warehouse in warehouses)//仓库
                {
                    Tree wareTree = new Tree();
                    wareTree.id = warehouse.WarehouseCode;
                    wareTree.text = "仓库：" + warehouse.WarehouseName;
                    wareTree.state = "open";
                    wareTree.attributes = "ware";

                    var areas = AreaRepository.GetQueryable().Where(a => a.Warehouse.WarehouseCode == warehouse.WarehouseCode)
                                                             .OrderBy(a => a.AreaCode).Select(a => a);
                    HashSet<Tree> areaSet = new HashSet<Tree>();
                    foreach (var area in areas)//库区
                    {
                        Tree areaTree = new Tree();
                        areaTree.id = area.AreaCode;
                        areaTree.text = "库区：" + area.AreaName;
                        areaTree.state = "open";
                        areaTree.attributes = "area";

                        var shelfs = ShelfRepository.GetQueryable().Where(s => s.Area.AreaCode == area.AreaCode)
                                                                   .OrderBy(s => s.ShelfCode).Select(s => s);
                        HashSet<Tree> shelfSet = new HashSet<Tree>();
                        foreach (var shelf in shelfs)//货架
                        {
                            Tree shelfTree = new Tree();
                            shelfTree.id = shelf.ShelfCode;
                            shelfTree.text = "货架：" + shelf.ShelfName;
                            shelfTree.attributes = "shelf";
                            shelfTree.state = "closed";
                            shelfSet.Add(shelfTree);
                        }
                        areaTree.children = shelfSet.ToArray();
                        areaSet.Add(areaTree);
                    }
                    wareTree.children = areaSet.ToArray();
                    wareSet.Add(wareTree);
                }
            }
            else
            {
                var cells = CellRepository.GetQueryable().Where(c => c.Shelf.ShelfCode == shelfCode)
                                                         .OrderBy(c => c.CellCode).Select(c => c);
                foreach (var cell in cells)//货位
                {
                    var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == cell.DefaultProductCode);
                    Tree cellTree = new Tree();
                    cellTree.id = cell.CellCode;
                    cellTree.text = "货位：" + cell.CellName;
                    cellTree.state = "open";
                    cellTree.attributes = "cell";
                    wareSet.Add(cellTree);
                }
            }
            return wareSet.ToArray();
        }

        public object FindCell(string parameter)
        {
            IQueryable<Cell> cellQuery = CellRepository.GetQueryable();
            var cell = cellQuery.Where(c => c.CellCode == parameter).OrderBy(b => b.CellCode).AsEnumerable()
                                .Select(b => new { b.CellCode, b.CellName, b.CellType, b.ShortName, b.Rfid, b.Layer,b.Col,b.ImgX,b.ImgY,b.IsSingle, b.MaxQuantity, b.Description, b.Warehouse.WarehouseName, b.Warehouse.WarehouseCode, b.Area.AreaCode, b.Area.AreaName, b.Shelf.ShelfCode, b.Shelf.ShelfName, DefaultProductCode = b.Product == null ? string.Empty : b.Product.ProductCode, ProductName = b.Product == null ? string.Empty : b.Product.ProductName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            return cell.First(c => c.CellCode == parameter);
        }

        public object GetSearch(string wareCode)
        {
            var warehouses = WarehouseRepository.GetQueryable().AsEnumerable();
            if (wareCode != null && wareCode != string.Empty)
            {
                warehouses = warehouses.Where(w => w.WarehouseCode == wareCode);
            }

            HashSet<WareTree> wareSet = new HashSet<WareTree>();
            foreach (var warehouse in warehouses)//仓库
            {
                WareTree wareTree = new WareTree();
                wareTree.Code = warehouse.WarehouseCode;
                wareTree.Name = "仓库：" + warehouse.WarehouseName;
                wareTree.WarehouseCode = warehouse.WarehouseCode;
                wareTree.WarehouseName = warehouse.WarehouseName;
                wareTree.Type = warehouse.WarehouseType;
                wareTree.Description = warehouse.Description;
                wareTree.IsActive = warehouse.IsActive == "1" ? "可用" : "不可用";
                wareTree.UpdateTime = warehouse.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss");
                wareTree.ShortName = warehouse.ShortName;
                wareTree.attributes = "ware";
                var areas = AreaRepository.GetQueryable().Where(a => a.Warehouse.WarehouseCode == warehouse.WarehouseCode)
                                                        .OrderBy(a => a.AreaCode).Select(a => a);
                HashSet<WareTree> areaSet = new HashSet<WareTree>();
                foreach (var area in areas)//库区
                {
                    WareTree areaTree = new WareTree();
                    areaTree.Code = area.AreaCode;
                    areaTree.Name = "库区：" + area.AreaName;
                    areaTree.AreaCode = area.AreaCode;
                    areaTree.AreaName = area.AreaName;
                    areaTree.WarehouseCode = area.Warehouse.WarehouseCode;
                    areaTree.WarehouseName = area.Warehouse.WarehouseName;
                    areaTree.Type = area.AreaType;
                    areaTree.Description = area.Description;
                    areaTree.IsActive = area.IsActive == "1" ? "可用" : "不可用";
                    areaTree.UpdateTime = area.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss");
                    areaTree.ShortName = area.ShortName;
                    areaTree.AllotInOrder = area.AllotInOrder;
                    areaTree.AllotOutOrder = area.AllotOutOrder;
                    areaTree.attributes = "area";
                    var shelfs = ShelfRepository.GetQueryable().Where(s => s.Area.AreaCode == area.AreaCode)
                                                               .OrderBy(s => s.ShelfCode).Select(s => s);
                    HashSet<WareTree> shelfSet = new HashSet<WareTree>();
                    foreach (var shelf in shelfs)//货架
                    {
                        WareTree shelfTree = new WareTree();
                        shelfTree.Code = shelf.ShelfCode;
                        shelfTree.Name = "货架：" + shelf.ShelfName;
                        shelfTree.ShelfCode = shelf.ShelfCode;
                        shelfTree.ShelfName = shelf.ShelfName;

                        shelfTree.WarehouseCode = shelf.Warehouse.WarehouseCode;
                        shelfTree.WarehouseName = shelf.Warehouse.WarehouseName;
                        shelfTree.AreaCode = shelf.Area.AreaCode;
                        shelfTree.AreaName = shelf.Area.AreaName;

                        shelfTree.Type = shelf.ShelfType;
                        shelfTree.Description = shelf.Description;
                        shelfTree.IsActive = shelf.IsActive == "1" ? "可用" : "不可用";
                        shelfTree.UpdateTime = shelf.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss");
                        shelfTree.ShortName = shelf.ShortName;
                        shelfTree.attributes = "shelf";
                        shelfSet.Add(shelfTree);
                    }
                    areaTree.children = shelfSet.ToArray();
                    areaSet.Add(areaTree);
                }
                wareTree.children = areaSet.ToArray();
                wareSet.Add(wareTree);
            }
            return wareSet.ToArray();
        }

        public object GetCell(string shelfCode)
        {
            HashSet<WareTree> wareSet = new HashSet<WareTree>();
            var cells = CellRepository.GetQueryable().Where(c => c.Shelf.ShelfCode == shelfCode)
                                                     .OrderBy(c => c.CellCode).Select(c => c);
            foreach (var cell in cells)//货位
            {
                var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == cell.DefaultProductCode);
                WareTree cellTree = new WareTree();
                cellTree.Code = cell.CellCode;
                cellTree.Name = "货位：" + cell.CellName;
                cellTree.CellCode = cell.CellCode;
                cellTree.CellName = cell.CellName;

                cellTree.WarehouseCode = cell.Warehouse.WarehouseCode;
                cellTree.WarehouseName = cell.Warehouse.WarehouseName;

                cellTree.AreaCode = cell.Area.AreaCode;
                cellTree.AreaName = cell.Area.AreaName;

                cellTree.ShelfCode = cell.Shelf.ShelfCode;
                cellTree.ShelfName = cell.Shelf.ShelfName;

                cellTree.Type = cell.CellType;
                cellTree.Description = cell.Description;
                cellTree.IsActive = cell.IsActive == "1" ? "可用" : "不可用";
                cellTree.UpdateTime = cell.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss");
                cellTree.ShortName = cell.ShortName;
                cellTree.Layer = cell.Layer;
                cellTree.MaxQuantity = cell.MaxQuantity;
                cellTree.ProductName = product.ProductName;
                cellTree.attributes = "cell";

                wareSet.Add(cellTree);
            }
            return wareSet;
        }

        #endregion
    }
}
