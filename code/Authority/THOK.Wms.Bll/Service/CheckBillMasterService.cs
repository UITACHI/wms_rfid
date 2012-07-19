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
    public class CheckBillMasterService : ServiceBase<CheckBillMaster>, ICheckBillMasterService
    {
        [Dependency]
        public ICheckBillMasterRepository CheckBillMasterRepository { get; set; }

        [Dependency]
        public ICellRepository CellRepository { get; set; }

        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }

        [Dependency]
        public IAreaRepository AreaRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICheckBillMasterService 成员

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status)
        {
            throw new NotImplementedException();
        }

        public new bool Add(CheckBillMaster inBillMaster)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(CheckBillMaster inBillMaster)
        {
            throw new NotImplementedException();
        }

        public object GetCheckCellInfo(string type, string id)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var storage = storageQuery.Where(s => s.cell.CellCode == type).Select(s => new {s.StorageCode,s.cell.CellCode,s.cell.CellName,s.product.ProductCode,s.product.ProductName });
            return storage.ToArray();
        }

        #endregion
        
    }
}
