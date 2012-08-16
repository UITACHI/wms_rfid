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
    public class CheckBillDetailService : ServiceBase<CheckBillDetail>, ICheckBillDetailService
    {
        [Dependency]
        public ICheckBillDetailRepository CheckBillDetailRepository { get; set; }

        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICheckBillDetailService 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            IQueryable<CheckBillDetail> checkBillDetailQuery = CheckBillDetailRepository.GetQueryable();
            if (BillNo != null && BillNo != string.Empty)
            {
                var checkBillDetail = checkBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i =>i);
                int total = checkBillDetail.Count();
                checkBillDetail = checkBillDetail.Skip((page - 1) * rows).Take(rows);

                var temp = checkBillDetail.ToArray().AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.Cell.CellCode,
                    i.Cell.CellName,
                    i.StorageCode,
                    i.Product.ProductCode,
                    i.Product.ProductName,
                    i.Unit.UnitCode,
                    i.Unit.UnitName,
                    Quantity = i.Quantity / i.Unit.Count,
                    RealProductCode = i.RealProduct.ProductCode,
                    RealProductName = i.RealProduct.ProductName,
                    RealUnitCode = i.RealUnit.UnitCode,
                    RealUnitName = i.RealUnit.UnitName,
                    OperatePersonCode = i.OperatePersonID == null ? string.Empty : i.OperatePerson.EmployeeCode,
                    OperatePersonName = i.OperatePersonID == null ? string.Empty : i.OperatePerson.EmployeeName,
                    StartTime = i.StartTime == null ? string.Empty : i.StartTime.ToString(),
                    FinishTime = i.FinishTime == null ? string.Empty : i.FinishTime.ToString(),
                    i.Status
                });
                return new { total, rows = temp.ToArray() };
            }
            return "";
        }

        public new bool CellAdd(string BillNo, string ware, string area, string shelf, string cell)
        {
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
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
            }
            var storages = storageQuery.Where(s => ware.Contains(s.Cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.Cell.Shelf.Area.AreaCode) || shelf.Contains(s.Cell.Shelf.ShelfCode) || cell.Contains(s.Cell.CellCode))
                                       .OrderBy(s => s.StorageCode).AsEnumerable()
                                       .Select(s => new { s.StorageCode, s.Cell.CellCode, s.Cell.CellName, s.Product.ProductCode, s.Product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
            foreach (var stor in storages)
            {
                var checkDetail = new CheckBillDetail();
                checkDetail.BillNo = BillNo;
                checkDetail.CellCode = stor.CellCode;
                checkDetail.StorageCode = stor.StorageCode;
                checkDetail.ProductCode = stor.ProductCode;
                checkDetail.UnitCode = "";
                checkDetail.Quantity = stor.Quantity;
                checkDetail.RealProductCode = stor.ProductCode;
                checkDetail.RealUnitCode = "";
                checkDetail.RealQuantity = stor.Quantity;
                checkDetail.Status = "1";
                CheckBillDetailRepository.Add(checkDetail);
                CheckBillDetailRepository.SaveChanges();
            }
            return true;            
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(CheckBillDetail inBillDetail)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
