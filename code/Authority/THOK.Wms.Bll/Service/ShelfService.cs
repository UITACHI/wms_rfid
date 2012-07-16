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
    public class ShelfService : ServiceBase<Shelf>, IShelfService
    {
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }

        [Dependency]
        public ICellRepository CellRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IShelfService 成员

        public object GetDetails(int page, int rows, string shelfCode)
        {
            IQueryable<Shelf> shelfQuery = ShelfRepository.GetQueryable();
            var shelf = shelfQuery.OrderBy(b => b.ShelfCode).AsEnumerable().Select(b => new { b.ShelfCode, b.ShelfName, b.ShelfType, b.ShortName, b.Description, b.area.AreaCode, b.area.AreaName, b.warehouse.WarehouseCode, b.warehouse.WarehouseName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (shelfCode != null)
            {
                shelf = shelf.Where(s => s.ShelfCode == shelfCode);
            }
            int total = shelf.Count();
            shelf = shelf.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = shelf.ToArray() };
        }

        public new bool Add(Shelf shelf)
        {
            var shelfAdd = new Shelf();
            var warehouse = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == shelf.WarehouseCode);
            var area = AreaRepository.GetQueryable().FirstOrDefault(a => a.AreaCode == shelf.AreaCode);
            shelfAdd.ShelfCode = shelf.ShelfCode;
            shelfAdd.ShelfName = shelf.ShelfName;
            shelfAdd.ShortName = shelf.ShortName;
            shelfAdd.ShelfType = shelf.ShelfType;
            shelfAdd.warehouse = warehouse;
            shelfAdd.area = area;
            shelfAdd.Description = shelf.Description;
            shelfAdd.IsActive = shelf.IsActive;
            shelfAdd.UpdateTime = DateTime.Now;

            ShelfRepository.Add(shelfAdd);
            ShelfRepository.SaveChanges();
            return true;
        }

        public bool Delete(string shelfCode)
        {
            var shelf = ShelfRepository.GetQueryable()
                .FirstOrDefault(s => s.ShelfCode == shelfCode);
            if (shelf != null)
            {
                //Del(CellRepository, shelf.Cells);
                ShelfRepository.Delete(shelf);
                ShelfRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(Shelf shelf)
        {
            var shelfSave = ShelfRepository.GetQueryable().FirstOrDefault(s => s.ShelfCode == shelf.ShelfCode);            
            var warehouse = WarehouseRepository.GetQueryable().FirstOrDefault(w => w.WarehouseCode == shelf.WarehouseCode);
            var area = AreaRepository.GetQueryable().FirstOrDefault(a => a.AreaCode == shelf.AreaCode);
            shelfSave.ShelfCode = shelfSave.ShelfCode;
            shelfSave.ShelfName = shelf.ShelfName;
            shelfSave.ShortName = shelf.ShortName;
            shelfSave.ShelfType = shelf.ShelfType;
            shelfSave.warehouse = warehouse;
            shelfSave.area = area;
            shelfSave.Description = shelf.Description;
            shelfSave.IsActive = shelf.IsActive;
            shelfSave.UpdateTime = DateTime.Now;

            ShelfRepository.SaveChanges();
            return true;
        }
       
        public object FindShelf(string parameter)
        {
            IQueryable<Shelf> shelfQuery = ShelfRepository.GetQueryable();
            var shelf = shelfQuery.Where(s=>s.ShelfCode==parameter).OrderBy(b => b.ShelfCode).AsEnumerable().Select(b => new { b.ShelfCode, b.ShelfName, b.ShelfType, b.ShortName, b.Description, b.area.AreaCode, b.area.AreaName, b.warehouse.WarehouseCode, b.warehouse.WarehouseName, IsActive = b.IsActive == "1" ? "可用" : "不可用", UpdateTime = b.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            return shelf.First(s => s.ShelfCode == parameter);
        }

        #endregion
    }
}
