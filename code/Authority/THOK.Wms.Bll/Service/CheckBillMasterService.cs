using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using System.Transactions;
using THOK.Wms.SignalR.Common;

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
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; } 

        [Dependency]
        public IProfitLossBillMasterRepository ProfitLossBillMasterRepository { get; set; }
        [Dependency]
        public IProfitLossBillDetailRepository ProfitLossBillDetailRepository { get; set; }

        [Dependency]
        public IProfitLossBillMasterService ProfitLossBillMasterService { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region 盘点单基础信息

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "1":
                    statusStr = "已录入";
                    break;
                case "2":
                    statusStr = "已审核";
                    break;
                case "3":
                    statusStr = "执行中";
                    break;
                case "4":
                    statusStr = "已盘点";
                    break;
                case "5":
                    statusStr = "已确认";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<CheckBillMaster> CheckBillMasterQuery = CheckBillMasterRepository.GetQueryable();
            //var checkBillMasters = CheckBillMasterQuery.Where(i => i.BillNo.Contains(BillNo) && i.OperatePerson.EmployeeName.Contains(OperatePersonCode));
            var checkBillMasters = CheckBillMasterQuery.Where(i => i.BillNo.Contains(BillNo)
                && i.Status!="5")
                .OrderByDescending(t => t.BillDate)
                .OrderByDescending(t => t.BillNo)
                .Select(i => i);
            if (!BillNo.Equals(string.Empty) && BillNo != null)
            {
                checkBillMasters = checkBillMasters.Where(i => i.BillNo.Contains(BillNo));
            }
            if (!beginDate.Equals(string.Empty))
            {
                DateTime begin = Convert.ToDateTime(beginDate);
                checkBillMasters = checkBillMasters.Where(i =>Convert.ToDateTime(i.BillDate) >= begin);
            }
            if (!endDate.Equals(string.Empty))
            {
                DateTime end = Convert.ToDateTime(endDate);
                checkBillMasters = checkBillMasters.Where(i => Convert.ToDateTime(i.BillDate) <= end);
            }
            if (!OperatePersonCode.Equals(string.Empty) && OperatePersonCode != null)
            {
                checkBillMasters = checkBillMasters.Where(i => i.OperatePerson.EmployeeCode.Contains(OperatePersonCode));
            }
            if (!Status.Equals(string.Empty))
            {
                checkBillMasters = checkBillMasters.Where(i => i.Status.Contains(Status));
            }
            if (!IsActive.Equals(string.Empty))
            {
                checkBillMasters = checkBillMasters.Where(i => i.IsActive.Contains(IsActive));
            }

            int total = checkBillMasters.Count();
            checkBillMasters = checkBillMasters.Skip((page - 1) * rows).Take(rows);

            var temp=checkBillMasters.ToArray().AsEnumerable().Select(i => new
                {
                    i.BillNo,
                    BillDate = i.BillDate.ToString("yyyy-MM-dd hh:mm:ss"),
                    i.Warehouse.WarehouseCode,
                    i.Warehouse.WarehouseName,
                    OperatePersonCode = i.OperatePerson.EmployeeCode,
                    OperatePersonName = i.OperatePerson.EmployeeName,
                    VerifyPersonCode = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeCode,
                    VerifyPersonName = i.VerifyPersonID == null ? string.Empty : i.VerifyPerson.EmployeeName,
                    BillTypeCode = i.BillType.BillTypeCode,
                    BillTypeName = i.BillType.BillTypeName,
                    VerifyDate = i.VerifyDate == null ? string.Empty : ((DateTime)i.VerifyDate).ToString("yyyy-MM-dd hh:mm:ss"),
                    Status = WhatStatus(i.Status),
                    IsActive = i.IsActive == "1" ? "可用" : "不可用",
                    Description = i.Description,
                    UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
                });
            return new { total, rows = temp.ToArray() };
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
            var checkbm = CheckBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            if (checkbm != null)
            {
                //Del(OutBillDetailRepository, ibm.OutBillAllots);
                foreach (var item in checkbm.CheckBillDetails.ToArray())
                {
                    item.Storage.IsLock = "0";
                }
                Del(CheckBillDetailRepository, checkbm.CheckBillDetails);
                CheckBillMasterRepository.Delete(checkbm);
                CheckBillMasterRepository.SaveChanges();
            }
            return true;
        }

        public bool Save(CheckBillMaster inBillMaster)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region 生成盘点单信息方法

        /// <summary>
        /// 生成盘点的主表ID
        /// </summary>
        /// <returns></returns>
        public object GetCheckBillNo()
        {
            IQueryable<CheckBillMaster> CheckMasterQuery = CheckBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            var inBillMaster = CheckMasterQuery.Where(i => i.BillNo.Contains(sysTime)).ToArray().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (inBillMaster.Count() == 0)
            {
                return System.DateTime.Now.ToString("yyMMdd") + "0001" + "CH";
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
                return System.DateTime.Now.ToString("yyMMdd") + newcode + "CH";
            }
        }

        /// <summary>
        /// 根据参数查询要生成的盘点数据  --货位查询预览
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
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
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
            }
            var storages = storageQuery.Where(s => (ware.Contains(s.Cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.Cell.Shelf.Area.AreaCode) || shelf.Contains(s.Cell.Shelf.ShelfCode) || cell.Contains(s.Cell.CellCode)) && s.Quantity > 0 && s.IsLock == "0")
                                       .OrderBy(s => s.StorageCode)
                                       .Select(s=>s);

            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);

            var temp = storages.ToArray().Select(s => new
                                       {
                                           s.StorageCode,
                                           s.Cell.CellCode,
                                           s.Cell.CellName,
                                           s.Product.ProductCode,
                                           s.Product.ProductName,
                                           s.Product.Unit.UnitCode,
                                           s.Product.Unit.UnitName,
                                           Quantity = s.Quantity / s.Product.Unit.Count,
                                           IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                           StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                           UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                       });
            return new { total, rows = temp.ToArray() };
        }

        /// <summary>
        /// 根据参数生成并保存盘点数据  --货位生成
        /// </summary>
        /// <param name="ware">仓库</param>
        /// <param name="area">库区</param>
        /// <param name="shelf">货架</param>
        /// <param name="cell">货位</param>
        /// <param name="UserName">登陆用户</param>
        /// <returns></returns>
        public bool CellAdd(string ware, string area, string shelf, string cell, string UserName, string billType, out string info)
        {
            bool result = false;
            info = string.Empty;
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.UserName == UserName);
            if (employee != null)
            {
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        #region ware 这个有值，就把这个值里面所有的仓库的货位的储存信息生成盘点单，一个仓库一个盘点单据

                        if (ware != null && ware != string.Empty)
                        {
                            ware = ware.Substring(0, ware.Length - 1);
                            var wares = wareQuery.Where(w => ware.Contains(w.WarehouseCode));

                            foreach (var item in wares.ToArray())
                            {
                                var storages = storageQuery.Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode && s.Quantity > 0 && s.IsLock == "0")
                                                           .OrderBy(s => s.StorageCode).AsEnumerable()
                                                           .Select(s => new
                                                           {
                                                               s.StorageCode,
                                                               s.Cell.CellCode,
                                                               s.Cell.CellName,
                                                               s.IsLock,
                                                               s.Product.ProductCode,
                                                               s.Product.ProductName,
                                                               s.Product.Unit.UnitCode,
                                                               s.Product.Unit.UnitName,
                                                               Quantity = s.Quantity / s.Product.Unit.Count,
                                                               s.Product.Unit.Count,
                                                               IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                                               StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                                               UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                                           });
                                if (storages.Count() > 0)
                                {
                                    string billNo = GetCheckBillNo().ToString();
                                    var check = new CheckBillMaster();
                                    check.BillNo = billNo;
                                    check.BillDate = DateTime.Now;
                                    check.BillTypeCode = billType;
                                    check.WarehouseCode = item.WarehouseCode;
                                    check.OperatePersonID = employee.ID;
                                    check.Status = "1";
                                    check.IsActive = "1";
                                    check.UpdateTime = DateTime.Now;

                                    CheckBillMasterRepository.Add(check);
                                    CheckBillMasterRepository.SaveChanges();

                                    foreach (var stor in storages.ToArray())
                                    {
                                        var checkDetail = new CheckBillDetail();
                                        checkDetail.BillNo = billNo;
                                        checkDetail.CellCode = stor.CellCode;
                                        checkDetail.StorageCode = stor.StorageCode;
                                        checkDetail.ProductCode = stor.ProductCode;
                                        checkDetail.UnitCode = stor.UnitCode;
                                        checkDetail.Quantity = stor.Quantity * stor.Count;
                                        checkDetail.RealProductCode = stor.ProductCode;
                                        checkDetail.RealUnitCode = stor.UnitCode;
                                        checkDetail.RealQuantity = stor.Quantity * stor.Count;
                                        checkDetail.Status = "1";

                                        CheckBillDetailRepository.Add(checkDetail);
                                        CheckBillDetailRepository.SaveChanges();

                                        var storage = storageQuery.FirstOrDefault(s => s.StorageCode == stor.StorageCode);
                                        storage.IsLock = "1";
                                        StorageRepository.SaveChanges();
                                        scope.Complete();
                                    }
                                    result = true;
                                }
                                else
                                {
                                    info = "所选择的货位无数据！";
                                }
                            }
                        }
                        #endregion

                        #region ware 这个无值，把全部仓库里面包含area，shelf，cell的数据生成盘点单，一个仓库一个盘点单据

                        if (area != null && area != string.Empty || shelf != null && shelf != string.Empty || cell != null && cell != string.Empty)
                        {
                            if (area != string.Empty)
                                area = area.Substring(0, area.Length - 1);
                            if (shelf != string.Empty)
                                shelf = shelf.Substring(0, shelf.Length - 1);
                            if (cell != string.Empty)
                                cell = cell.Substring(0, cell.Length - 1);

                            var warehouses = wareQuery.OrderBy(w => w.WarehouseCode);

                            foreach (var item in warehouses.ToArray())
                            {
                                var storages = storageQuery.ToList().Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode && (area.Contains(s.Cell.Shelf.Area.AreaCode) || shelf.Contains(s.Cell.Shelf.ShelfCode) || cell.Contains(s.Cell.CellCode)) && s.Quantity > 0 && s.IsLock == "0")
                                                           .OrderBy(s => s.StorageCode).AsEnumerable()
                                                           .Select(s => new
                                                           {
                                                               s.StorageCode,
                                                               s.Cell.CellCode,
                                                               s.Cell.CellName,
                                                               s.Product.ProductCode,
                                                               s.Product.ProductName,
                                                               s.Product.Unit.UnitCode,
                                                               s.Product.Unit.UnitName,
                                                               Quantity = s.Quantity / s.Product.Unit.Count,
                                                               s.Product.Unit.Count,
                                                               IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                                               StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                                               UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                                           });
                                if (storages.Count() > 0)
                                {
                                    string billNo = GetCheckBillNo().ToString();
                                    var check = new CheckBillMaster();
                                    check.BillNo = billNo;
                                    check.BillDate = DateTime.Now;
                                    check.BillTypeCode = billType;
                                    check.WarehouseCode = item.WarehouseCode;
                                    check.OperatePersonID = employee.ID;
                                    check.Status = "1";
                                    check.IsActive = "1";
                                    check.UpdateTime = DateTime.Now;

                                    CheckBillMasterRepository.Add(check);
                                    CheckBillMasterRepository.SaveChanges();

                                    foreach (var stor in storages.ToArray())
                                    {
                                        var checkDetail = new CheckBillDetail();
                                        checkDetail.BillNo = billNo;
                                        checkDetail.CellCode = stor.CellCode;
                                        checkDetail.StorageCode = stor.StorageCode;
                                        checkDetail.ProductCode = stor.ProductCode;
                                        checkDetail.UnitCode = stor.UnitCode;
                                        checkDetail.Quantity = stor.Quantity * stor.Count;
                                        checkDetail.RealProductCode = stor.ProductCode;
                                        checkDetail.RealUnitCode = stor.UnitCode;
                                        checkDetail.RealQuantity = stor.Quantity * stor.Count;
                                        checkDetail.Status = "1";
                                        CheckBillDetailRepository.Add(checkDetail);
                                        CheckBillDetailRepository.SaveChanges();

                                        var storage = storageQuery.FirstOrDefault(s => s.StorageCode == stor.StorageCode);
                                        storage.IsLock = "1";
                                        StorageRepository.SaveChanges();
                                        scope.Complete();
                                    }
                                    result = true;
                                }
                            }
                        }
                        #endregion

                        scope.Complete();
                    }
                }
                catch (Exception e)
                {
                    info = e.Message;
                }
            }
            else
            {
                info = "当前登陆用户为空！请重新登陆！";
            }
            return result;
        }

        /// <summary>
        /// 根据参数查询要生成的盘点数据  --产品查询预览
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

                var storages = storageQuery.Where(s => s.ProductCode != null && products.Contains(s.ProductCode) && s.Quantity > 0 && s.IsLock == "0")
                                      .OrderBy(s => s.StorageCode)
                                      .Select(s =>s);
                int total = storages.Count();
                storages = storages.Skip((page - 1) * rows).Take(rows);

                var temp = storages.ToArray().Select(s => new
                {
                    s.StorageCode,
                    s.Cell.CellCode,
                    s.Cell.CellName,
                    s.Product.ProductCode,
                    s.Product.ProductName,
                    s.Product.Unit.UnitCode,
                    s.Product.Unit.UnitName,
                    Quantity = s.Quantity / s.Product.Unit.Count,
                    IsActive = s.IsActive == "1" ? "可用" : "不可用",
                    StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                    UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                });
                return new { total, rows = temp.ToArray() };
            }
            return null;
        }

        /// <summary>
        /// 根据参数生成并保存盘点数据  --产品生成
        /// </summary>
        /// <param name="products">产品数据</param>
        /// <param name="UserName">登陆用户</param>
        /// <returns></returns>
        public bool ProductAdd(string products, string UserName, string billType, out string info)
        {
            bool result = false;
            info = string.Empty;
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.UserName == UserName);
            if (employee != null)
            {
                if (products != null && products != string.Empty)
                {
                    products = products.Substring(0, products.Length - 1);
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            #region products 这个有值，就把这个值里面所有的卷烟信息所在的仓库的货位的储存信息生成盘点单，一个仓库一个盘点单据
                            var warehouses = wareQuery.OrderBy(w => w.WarehouseCode);
                            foreach (var item in warehouses.ToArray())
                            {
                                var storages = storageQuery.Where(s =>s.ProductCode!=null&& products.Contains(s.ProductCode) && s.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode && s.Quantity > 0 && s.IsLock == "0")
                                                           .OrderBy(s => s.StorageCode).AsEnumerable()
                                                           .Select(s => new
                                                            {
                                                                s.StorageCode,
                                                                s.Cell.CellCode,
                                                                s.Cell.CellName,
                                                                s.Product.ProductCode,
                                                                s.Product.ProductName,
                                                                s.Product.Unit.UnitCode,
                                                                s.Product.Unit.UnitName,
                                                                Quantity = s.Quantity / s.Product.Unit.Count,
                                                                s.Product.Unit.Count,
                                                                IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                                                StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                                                UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                                            });
                                if (storages.Count() > 0)
                                {
                                    string billNo = GetCheckBillNo().ToString();
                                    var check = new CheckBillMaster();
                                    check.BillNo = billNo;
                                    check.BillDate = DateTime.Now;
                                    check.BillTypeCode = billType;
                                    check.WarehouseCode = item.WarehouseCode;
                                    check.OperatePersonID = employee.ID;
                                    check.Status = "1";
                                    check.IsActive = "1";
                                    check.UpdateTime = DateTime.Now;

                                    CheckBillMasterRepository.Add(check);
                                    CheckBillMasterRepository.SaveChanges();

                                    foreach (var stor in storages.ToArray())
                                    {
                                        var checkDetail = new CheckBillDetail();
                                        checkDetail.BillNo = billNo;
                                        checkDetail.CellCode = stor.CellCode;
                                        checkDetail.StorageCode = stor.StorageCode;
                                        checkDetail.ProductCode = stor.ProductCode;
                                        checkDetail.UnitCode = stor.UnitCode;
                                        checkDetail.Quantity = stor.Quantity * stor.Count;
                                        checkDetail.RealProductCode = stor.ProductCode;
                                        checkDetail.RealUnitCode = stor.UnitCode;
                                        checkDetail.RealQuantity = stor.Quantity * stor.Count;
                                        checkDetail.Status = "1";
                                        CheckBillDetailRepository.Add(checkDetail);
                                        CheckBillDetailRepository.SaveChanges();

                                        var storage = storageQuery.FirstOrDefault(s => s.StorageCode == stor.StorageCode);
                                        storage.IsLock = "1";
                                        StorageRepository.SaveChanges();
                                    }
                                    result = true;
                                }
                                else
                                {
                                    info = "所选择的产品无数据！";
                                }
                            }
                            #endregion
                            scope.Complete();
                        }
                    }
                    catch (Exception e)
                    {
                        info = e.Message;
                    }
                }
                else
                {
                    info = "产品信息为空！无法生成！";
                }
            }
            else
            {
                info = "当前登陆用户为空！请重新登陆！";
            }
            return result;
        }

        /// <summary>
        /// 根据参数查询要生成的盘点数据  --异动查询预览
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
            if (beginDate == string.Empty || beginDate == null)
            {
                beginDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
            }
            if (endDate == string.Empty || endDate == null)
            {
                endDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            DateTime begin = Convert.ToDateTime(beginDate);
            DateTime end = Convert.ToDateTime(endDate);

            var inCells = inAllotQuery.Where(i => i.FinishTime >= begin && i.FinishTime <= end).OrderBy(i => i.CellCode).Select(i => i.CellCode);
            var outCells = outAllotQuery.Where(o => o.FinishTime >= begin && o.FinishTime <= end).OrderBy(o => o.CellCode).Select(o => o.CellCode);
            var moveInCells = moveBillQuery.Where(m => m.FinishTime >= begin && m.FinishTime <= end).OrderBy(m => m.InCell.CellCode).Select(m => m.InCell.CellCode);
            var moveOutCells = moveBillQuery.Where(m => m.FinishTime >= begin && m.FinishTime <= end).OrderBy(m => m.OutCell.CellCode).Select(m => m.OutCell.CellCode);
            var storages = storageQuery.Where(s => s.Quantity > 0 && s.IsLock == "0" && (inCells.Any(i => i == s.CellCode) || outCells.Any(o => o == s.CellCode) || moveInCells.Any(mi => mi == s.CellCode) || moveOutCells.Any(mo => mo == s.CellCode)))
                                       .OrderBy(s => s.ProductCode)
                                       .Select(s => s);
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);

            var temp = storages.ToArray().Select(s => new
                {
                    s.StorageCode,
                    s.Cell.CellCode,
                    s.Cell.CellName,
                    s.Product.ProductCode,
                    s.Product.ProductName,
                    s.Product.Unit.UnitCode,
                    s.Product.Unit.UnitName,
                    Quantity = s.Quantity / s.Product.Unit.Count,
                    IsActive = s.IsActive == "1" ? "可用" : "不可用",
                    StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                    UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                });
            return new { total, rows = temp.ToArray() };
        }

        /// <summary>
        /// 根据参数生成并保存盘点数据  --异动生成
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="UserName">登陆用户</param>
        /// <returns></returns>
        public bool ChangedAdd(string beginDate, string endDate, string UserName, string billType, out string info)
        {
            bool result = false;
            info = string.Empty;
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            IQueryable<InBillAllot> inAllotQuery = InBillAllotRepository.GetQueryable();
            IQueryable<OutBillAllot> outAllotQuery = OutBillAllotRepository.GetQueryable();
            IQueryable<MoveBillDetail> moveBillQuery = MoveBillDetailRepository.GetQueryable();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.UserName == UserName);
            if (employee != null)
            {
                if (beginDate == string.Empty || beginDate == null)
                {
                    beginDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                }
                if (endDate == string.Empty || endDate == null)
                {
                    endDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                DateTime begin = Convert.ToDateTime(beginDate);
                DateTime end = Convert.ToDateTime(endDate);
                try
                {
                    using (var scope = new TransactionScope())
                    {
                        #region 循环所有仓库的订单，一个仓库一个盘点单据
                        var warehouses = wareQuery.OrderBy(w => w.WarehouseCode);
                        foreach (var item in warehouses.ToArray())
                        {
                            var inCells = inAllotQuery.Where(i => i.FinishTime >= begin && i.FinishTime <= end && i.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(i => i.CellCode).Select(i => i.CellCode);
                            var outCells = outAllotQuery.Where(o => o.FinishTime >= begin && o.FinishTime <= end && o.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(o => o.CellCode).Select(o => o.CellCode);
                            var moveInCells = moveBillQuery.Where(m => m.FinishTime >= begin && m.FinishTime <= end && m.InCell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(m => m.InCell.CellCode).Select(m => m.InCell.CellCode);
                            var moveOutCells = moveBillQuery.Where(m => m.FinishTime >= begin && m.FinishTime <= end && m.OutCell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(m => m.OutCell.CellCode).Select(m => m.OutCell.CellCode);
                            var storages = storageQuery.ToList().Where(s => s.Quantity > 0 && s.IsLock == "0" && (inCells.Any(i => i == s.CellCode) || outCells.Any(o => o == s.CellCode) || moveInCells.Any(mi => mi == s.CellCode) || moveOutCells.Any(mo => mo == s.CellCode)))
                                                       .OrderBy(s => s.ProductCode).AsEnumerable()
                                                       .Select(s => new
                                                       {
                                                           s.StorageCode,
                                                           s.Cell.CellCode,
                                                           s.Cell.CellName,
                                                           s.Product.ProductCode,
                                                           s.Product.ProductName,
                                                           s.Product.Unit.UnitCode,
                                                           s.Product.Unit.UnitName,
                                                           Quantity = s.Quantity / s.Product.Unit.Count,
                                                           s.Product.Unit.Count,
                                                           IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                                           StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                                           UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                                       });
                            if (storages.Count() > 0)
                            {
                                string billNo = GetCheckBillNo().ToString();
                                var check = new CheckBillMaster();
                                check.BillNo = billNo;
                                check.BillDate = DateTime.Now;
                                check.BillTypeCode = billType;
                                check.WarehouseCode = item.WarehouseCode;
                                check.OperatePersonID = employee.ID;
                                check.Status = "1";
                                check.IsActive = "1";
                                check.UpdateTime = DateTime.Now;

                                CheckBillMasterRepository.Add(check);
                                CheckBillMasterRepository.SaveChanges();

                                foreach (var stor in storages.ToArray())
                                {
                                    var checkDetail = new CheckBillDetail();
                                    checkDetail.BillNo = billNo;
                                    checkDetail.CellCode = stor.CellCode;
                                    checkDetail.StorageCode = stor.StorageCode;
                                    checkDetail.ProductCode = stor.ProductCode;
                                    checkDetail.UnitCode = stor.UnitCode;
                                    checkDetail.Quantity = stor.Quantity * stor.Count;
                                    checkDetail.RealProductCode = stor.ProductCode;
                                    checkDetail.RealUnitCode = stor.UnitCode;
                                    checkDetail.RealQuantity = stor.Quantity * stor.Count;
                                    checkDetail.Status = "1";
                                    CheckBillDetailRepository.Add(checkDetail);
                                    CheckBillDetailRepository.SaveChanges();

                                    var storage = storageQuery.FirstOrDefault(s => s.StorageCode == stor.StorageCode);
                                    storage.IsLock = "1";
                                    StorageRepository.SaveChanges();
                                }
                                result = true;
                            }
                            else
                            {
                                info = "所选择查询的时间无数据！";
                            }
                        }
                        #endregion
                        scope.Complete();
                    }
                }
                catch (Exception e)
                {
                    info = e.Message;
                }
            }
            return result;
        }

        #endregion

        #region 盘点审核、反审、确认

        /// <summary>
        /// 盘点审核
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="userName">登陆用户</param>
        /// <returns></returns>
        public bool Audit(string billNo, string userName)
        {
            bool result = false;
            var checkbm = CheckBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (employee != null)
            {
                if (checkbm != null && checkbm.Status == "1")
                {
                    checkbm.Status = "2";
                    checkbm.VerifyDate = DateTime.Now;
                    checkbm.UpdateTime = DateTime.Now;
                    checkbm.VerifyPersonID = employee.ID;
                    CheckBillMasterRepository.SaveChanges();
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 盘点反审
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns></returns>
        public bool AntiTrial(string billNo)
        {
            bool result = false;
            var outbm = CheckBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            if (outbm != null && outbm.Status == "2")
            {
                outbm.Status = "1";
                outbm.VerifyDate = null;
                outbm.UpdateTime = DateTime.Now;
                outbm.VerifyPersonID = null;
                CheckBillMasterRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 盘点确认
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns></returns>
        public bool confirmCheck(string billNo, string userName, out string errorInfo)
        {
            bool result = false;
            errorInfo = string.Empty;
            var checkbm = CheckBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            var checkDetail = CheckBillDetailRepository.GetQueryable().Where(c => c.BillNo == checkbm.BillNo
                                                                    && c.ProductCode == c.RealProductCode
                                                                    && c.Quantity != c.RealQuantity
                                                                    && c.Status == "2");
            using (var scope = new TransactionScope())
            {
                try
                {
                    if (checkDetail.Count() > 0)
                    {
                        string billno = GenProfitLossBillNo(userName).ToString();
                        //添加损益主表
                        var pbm = new ProfitLossBillMaster();
                        var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
                        if (employee != null)
                        {
                            pbm.BillNo = billno;
                            pbm.BillDate = DateTime.Now;
                            pbm.BillTypeCode = "5002";
                            pbm.WarehouseCode = checkbm.WarehouseCode;
                            pbm.OperatePersonID = employee.ID;
                            pbm.Status = "1";
                            pbm.IsActive = "1";
                            pbm.UpdateTime = DateTime.Now;

                            ProfitLossBillMasterRepository.Add(pbm);
                            ProfitLossBillMasterRepository.SaveChanges();
                        }

                        //添加损益细表
                        foreach (var item in checkDetail.ToArray())
                        {
                            decimal differQuantity = item.RealQuantity - item.Quantity;//损益数量
                            if (Locker.LockNoEmptyStorage(item.Storage, item.Product) != null)//锁库存
                            {
                                var pbd = new ProfitLossBillDetail();
                                pbd.BillNo = billno;
                                pbd.CellCode = item.CellCode;
                                pbd.StorageCode = item.StorageCode;
                                pbd.ProductCode = item.ProductCode;
                                pbd.UnitCode = item.UnitCode;
                                pbd.Price = item.Product != null ? item.Product.CostPrice : 0;
                                pbd.Quantity = differQuantity;

                                if (differQuantity > 0)
                                {
                                    item.Storage.InFrozenQuantity += differQuantity;
                                }
                                else
                                {
                                    item.Storage.OutFrozenQuantity += Math.Abs(differQuantity);
                                }
                                ProfitLossBillDetailRepository.Add(pbd);
                                item.Storage.LockTag = string.Empty;
                                ProfitLossBillDetailRepository.SaveChanges();
                            }
                            scope.Complete();
                        }
                    }

                    var checkBillDetail = CheckBillDetailRepository.GetQueryable().Where(c => c.BillNo == checkbm.BillNo);//解锁盘点锁定
                    foreach (var item in checkBillDetail.ToArray())
                    {
                        item.Storage.IsLock = "0";
                    }
                    if (checkbm != null && checkbm.Status == "4")
                    {
                        checkbm.Status = "5";
                        checkbm.VerifyDate = DateTime.Now;
                        checkbm.UpdateTime = DateTime.Now;
                        CheckBillMasterRepository.SaveChanges();
                        result = true;
                    }
                }
                catch (Exception e)
                {
                    errorInfo = "确认盘点损益失败！原因：" + e.Message;
                }
                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// 生成损益主单单号
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public object GenProfitLossBillNo(string userName)
        {
            IQueryable<ProfitLossBillMaster> profitLossBillMasterQuery = ProfitLossBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            string billNo = "";
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            var profitLossBillMaster = profitLossBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).ToArray().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (profitLossBillMaster.Count() == 0)
            {
                billNo = System.DateTime.Now.ToString("yyMMdd") + "0001" + "PL";
            }
            else
            {
                string billNoStr = profitLossBillMaster.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                billNo = System.DateTime.Now.ToString("yyMMdd") + newcode + "PL";
            }

            return billNo;
        }

        #endregion
    }
}
