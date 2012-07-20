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
        public ICheckBillDetailRepository CheckBillDetailRepository { get; set; }

        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ICheckBillMasterService 成员

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status)
        {
            throw new NotImplementedException();
        }

        public bool Add(string billNo, string wareCode)
        {
            Guid empanyid = new Guid("d9f369dd-d793-41f5-a191-815503766e94");
            var check = new CheckBillMaster();
            check.BillNo = "12070001CK";
            check.BillDate = DateTime.Now;
            check.BillTypeCode = "1";
            check.WarehouseCode = "CK001";
            check.OperatePersonID = empanyid;
            check.VerifyDate = DateTime.Now;
            check.Status = "1";
            check.IsActive = "1";
            check.UpdateTime = DateTime.Now;

            CheckBillMasterRepository.Add(check);
            CheckBillMasterRepository.SaveChanges();
            return true;
        }

        public bool Delete(string BillNo)
        {
            throw new NotImplementedException();
        }

        public bool Save(CheckBillMaster inBillMaster)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成获取盘点的主表ID
        /// </summary>
        /// <returns></returns>
        public object GetCheckBillNo()
        {
            IQueryable<CheckBillMaster> CheckMasterQuery = CheckBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            var inBillMaster = CheckMasterQuery.Where(i => i.BillNo.Contains(sysTime)).AsEnumerable().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (inBillMaster.Count() == 0)
            {
                return System.DateTime.Now.ToString("yyMMdd") + "0001" + "CK";
            }
            else
            {
                string billNoStr = inBillMaster.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                return System.DateTime.Now.ToString("yyMMdd") + newcode + "CK";
            }
        }

        /// <summary>
        /// 根据参数保存盘点数据
        /// </summary>
        /// <param name="ware">仓库</param>
        /// <param name="area">库区</param>
        /// <param name="shelf">货架</param>
        /// <param name="cell">货位</param>
        /// <returns></returns>
        public bool CellAdd(string ware, string area, string shelf, string cell)
        {
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            Guid empanyid = new Guid("d9f369dd-d793-41f5-a191-815503766e94");

            #region ware 这个有值，就把这个值里面所有的仓库的货位的储存信息生成盘点单，一个仓库一个盘点单据

            if (ware != null && ware != string.Empty)
            {
                ware = ware.Substring(0, ware.Length - 1);
                var wares = wareQuery.Where(w => ware.Contains(w.WarehouseCode));

                foreach (var item in wares)
                {
                    var storages = storageQuery.Where(s => s.cell.Shelf.Area.Warehouse.WarehouseCode.Contains(item.WarehouseCode))
                                               .OrderBy(s => s.StorageCode).AsEnumerable()
                                               .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
                    if (storages.Count() > 0)
                    {
                        string billNo = GetCheckBillNo().ToString();
                        var check = new CheckBillMaster();
                        check.BillNo = billNo;
                        check.BillDate = DateTime.Now;
                        check.BillTypeCode = "1";
                        check.WarehouseCode = item.WarehouseCode;
                        check.OperatePersonID = empanyid;
                        check.Status = "1";
                        check.IsActive = "1";
                        check.UpdateTime = DateTime.Now;

                        CheckBillMasterRepository.Add(check);
                        CheckBillMasterRepository.SaveChanges();

                        foreach (var stor in storages)
                        {
                            var checkDetail = new CheckBillDetail();
                            checkDetail.BillNo = billNo;
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
                    }
                }
            }

            #endregion

            #region ware 这个无值，把全部仓库里面包含area，shelf，cell的数据生成盘点单，一个仓库一个盘点单据

            var warehouses = wareQuery.OrderBy(w => w.WarehouseCode);

            foreach (var item in warehouses)
            {
                var storages = storageQuery.Where(s => s.cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode || area.Contains(s.cell.Shelf.Area.AreaCode) || shelf.Contains(s.cell.Shelf.ShelfCode) || cell.Contains(s.cell.CellCode))
                                           .OrderBy(s => s.StorageCode).AsEnumerable()
                                           .Select(s => new { s.StorageCode, s.cell.CellCode, s.cell.CellName, s.product.ProductCode, s.product.ProductName, s.Quantity, IsActive = s.IsActive == "1" ? "可用" : "不可用", StorageTime = s.StorageTime.ToString("yyyy-MM-dd"), UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd") });
                if (storages.Count() > 0)
                {
                    string billNo = GetCheckBillNo().ToString();
                    var check = new CheckBillMaster();
                    check.BillNo = billNo;
                    check.BillDate = DateTime.Now;
                    check.BillTypeCode = "1";
                    check.WarehouseCode = item.WarehouseCode;
                    check.OperatePersonID = empanyid;
                    check.Status = "1";
                    check.IsActive = "1";
                    check.UpdateTime = DateTime.Now;

                    CheckBillMasterRepository.Add(check);
                    CheckBillMasterRepository.SaveChanges();

                    if (area != null && area != string.Empty || shelf != null && shelf != string.Empty || cell != null && cell != string.Empty)
                    {
                        if (area != string.Empty)
                            area = area.Substring(0, area.Length - 1);
                        if (shelf != string.Empty)
                            shelf = shelf.Substring(0, shelf.Length - 1);
                        if (cell != string.Empty)
                            cell = cell.Substring(0, cell.Length - 1);
                    }

                    foreach (var stor in storages)
                    {
                        var checkDetail = new CheckBillDetail();
                        checkDetail.BillNo = billNo;
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
                }
            }

            #endregion

            return true;
        }

        #endregion
    }
}
