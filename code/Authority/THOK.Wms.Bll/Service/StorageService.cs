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

        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }

        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }

        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }

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
                storages = storages.Where(s => s.cell.Shelf.Area.Warehouse.WarehouseCode == id);
            }
            else if (type == "area")
            {
                storages = storageQuery.Where(s => s.cell.Shelf.Area.AreaCode == id);
            }
            else if (type == "shelf")
            {
                storages = storageQuery.Where(s => s.cell.Shelf.ShelfCode == id);
            }
            else if (type == "cell")
            {
                storages = storageQuery.Where(s => s.cell.CellCode == id);
            }

            var temp = storages.AsEnumerable().Select(s => new
           {
               s.StorageCode,
               s.cell.CellCode,
               s.cell.CellName,
               s.product.ProductCode,
               s.product.ProductName,
               s.Quantity,
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
                s.cell.CellCode,
                s.cell.CellName,
                s.product.ProductCode,
                s.product.ProductName,
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

                storages = storageQuery.ToList().Where(s => ware.Contains(s.cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.cell.Shelf.Area.AreaCode) || shelf.Contains(s.cell.Shelf.ShelfCode) || cell.Contains(s.cell.CellCode))
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new
                                       {
                                           s.StorageCode,
                                           s.cell.CellCode,
                                           s.cell.CellName,
                                           s.product.ProductCode,
                                           s.product.ProductName,
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

                var storages = storageQuery.ToList().Where(s => products.Contains(s.product.ProductCode))
                                      .OrderBy(s => s.StorageCode).AsEnumerable()
                                      .Select(s => new
                                      {
                                          s.StorageCode,
                                          s.cell.CellCode,
                                          s.cell.CellName,
                                          s.product.ProductCode,
                                          s.product.ProductName,
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

        /// <summary>
        /// 根据参数获取要生成的盘点数据  --货位变动
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <returns></returns>
        public object GetChangedCellDetails(int page, int rows, string beginDate, string endDate)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            IQueryable<InBillAllot> inAllotQuery = InBillAllotRepository.GetQueryable();
            IQueryable<OutBillAllot> outAllotQuery = OutBillAllotRepository.GetQueryable();
            IQueryable<MoveBillDetail> moveBillQuery = MoveBillDetailRepository.GetQueryable();
            if (beginDate == string.Empty && beginDate == null)
            { 
                
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}
