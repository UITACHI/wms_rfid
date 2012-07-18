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

        public object GetDetails(int page, int rows)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storage = storageQuery.OrderBy(s=>s.StorageCode).AsEnumerable().Select(s => new {s.StorageCode,s.cell.CellCode,s.cell.CellName,s.product.ProductCode,s.product.ProductName,s.Quantity,IsActive = s.IsActive == "1" ? "可用" : "不可用",StorageTime=s.StorageTime.ToString("yyyy-MM-dd"),UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            int total = storage.Count();
            storage = storage.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storage.ToArray() };
        }

        #endregion
    }
}
