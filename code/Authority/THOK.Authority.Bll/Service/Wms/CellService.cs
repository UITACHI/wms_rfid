using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Bll.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces.Wms;
using THOK.Authority.Bll.Models.Authority;

namespace THOK.Authority.Bll.Service.Wms
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
            var cell = cellQuery.OrderBy(b => b.CellCode).AsEnumerable().Select(b => new { b.CellCode, b.CellName, b.CellType, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
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
            cellAdd.Rfid = cell.Rfid;           
            cellAdd.warehouse = warehouse;
            cellAdd.area = area;
            cellAdd.shelf = shelf;
            cellAdd.product = product;
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
            cellSave.CellCode = cell.CellCode;
            cellSave.CellName = cell.CellName;
            cellSave.ShortName = cell.ShortName;
            cellSave.CellType = cell.CellType;
            cellSave.Layer = cell.Layer;
            cellSave.Rfid = cell.Rfid;
            cellSave.warehouse = warehouse;
            cellSave.area = area;
            cellSave.shelf = shelf;
            cellSave.product = product;
            cellSave.MaxQuantity = cell.MaxQuantity;
            cellSave.IsSingle = cell.IsSingle;
            cellSave.Description = cell.Description;
            cellSave.IsActive = cell.IsActive;
            cellSave.UpdateTime = DateTime.Now;

            CellRepository.SaveChanges();
            return true;
        }
        
        public object GetSearch(string shelfCode)
        {
            var warehouses = WarehouseRepository.GetQueryable().AsEnumerable();
            HashSet<AreaCell> wareSet = new HashSet<AreaCell>();
            foreach (var warehouse in warehouses)//仓库
            {
                AreaCell wareTree = new AreaCell();
                wareTree.WarehouseCode = warehouse.WarehouseCode;
                wareTree.WarehouseName = warehouse.WarehouseName;
                wareTree.ShortName = warehouse.ShortName;
                wareTree.Type = warehouse.WarehouseType;
                wareTree.Description = warehouse.Description;
                wareTree.IsActive = warehouse.IsActive;
                wareTree.UpdateTime = warehouse.UpdateTime.ToString();
                wareTree.isType = "0";

                var areas = AreaRepository.GetQueryable().Where(a => a.warehouse.WarehouseCode == warehouse.WarehouseCode)
                                                        .OrderBy(a => a.AreaCode).Select(a => a);
                HashSet<AreaCell> areaSet = new HashSet<AreaCell>();
                foreach (var area in areas)//库区
                {
                    AreaCell areaTree = new AreaCell();
                    areaTree.WarehouseCode = area.AreaCode;
                    areaTree.WarehouseName = area.AreaName;
                    areaTree.AreaCode = area.AreaCode;
                    areaTree.AreaName = area.AreaName;
                    areaTree.ShortName = area.ShortName;
                    areaTree.Type = area.AreaType;
                    areaTree.Description = area.Description;
                    areaTree.IsActive = area.IsActive;
                    areaTree.UpdateTime = area.UpdateTime.ToString();
                    areaTree.isType = "1";

                    var shelfs = ShelfRepository.GetQueryable().Where(s => s.area.AreaCode == area.AreaCode)
                                                               .OrderBy(s => s.ShelfCode).Select(s => s);
                    HashSet<AreaCell> shelfSet = new HashSet<AreaCell>();
                    foreach (var shelf in shelfs)//货架
                    {
                        AreaCell shelfTree = new AreaCell();
                        shelfTree.WarehouseCode = shelf.ShelfCode;
                        shelfTree.WarehouseName = shelf.ShelfName;
                        shelfTree.ShelfCode = shelf.ShelfCode;
                        shelfTree.ShelfName = shelf.ShelfName;
                        shelfTree.ShortName = shelf.ShortName;
                        shelfTree.Type = shelf.ShelfType;
                        shelfTree.Description = shelf.Description;
                        shelfTree.IsActive = shelf.IsActive;
                        shelfTree.UpdateTime = shelf.UpdateTime.ToString();
                        shelfTree.isType = "2";

                        var cells = CellRepository.GetQueryable().Where(c => c.shelf.ShelfCode == shelf.ShelfCode)
                                                                 .OrderBy(c => c.CellCode).Select(c => c);
                        HashSet<AreaCell> cellSet = new HashSet<AreaCell>();
                        foreach (var cell in cells)//货位
                        {
                            var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == cell.DefaultProductCode);
                            AreaCell cellTree = new AreaCell();
                            cellTree.WarehouseCode = cell.CellCode;
                            cellTree.WarehouseName = cell.CellName;
                            cellTree.CellCode = cell.CellCode;
                            cellTree.CellName = cell.CellName;
                            cellTree.ShortName = cell.ShortName;
                            cellTree.DefaultProductCode =product.ProductName;
                            cellTree.Type = cell.CellType;
                            cellTree.Description = cell.Description;
                            cellTree.IsActive = cell.IsActive;
                            cellTree.UpdateTime = cell.UpdateTime.ToString();
                            cellTree.isType = "3";
                            cellSet.Add(cellTree);
                        }
                        shelfTree.children = cellSet.ToArray();
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

        #endregion
    }
}
