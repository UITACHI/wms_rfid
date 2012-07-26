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
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
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

        #region ICheckBillMasterService 成员

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
                    statusStr = "已分配";
                    break;
                case "4":
                    statusStr = "已确认";
                    break;
                case "5":
                    statusStr = "执行中";
                    break;
                case "6":
                    statusStr = "已入库";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<CheckBillMaster> CheckBillMasterQuery = CheckBillMasterRepository.GetQueryable();
            var checkBillMasters = CheckBillMasterQuery.Where(i => i.BillNo.Contains(BillNo) && i.OperatePerson.EmployeeCode.Contains(OperatePersonCode));
            if (!beginDate.Equals(string.Empty))
            {
                checkBillMasters = checkBillMasters.Where(i => i.BillDate > Convert.ToDateTime(beginDate));
            }

            if (!endDate.Equals(string.Empty))
            {
                checkBillMasters = checkBillMasters.Where(i => i.BillDate < Convert.ToDateTime(endDate));
            }

            if (!Status.Equals(string.Empty))
            {
                checkBillMasters = checkBillMasters.Where(i => i.Status.Contains(Status) && i.Status != "6");
            }

            if (!IsActive.Equals(string.Empty))
            {
                checkBillMasters = checkBillMasters.Where(i => i.IsActive.Contains(IsActive));
            }

            var temp = checkBillMasters.Where(t => t.BillNo.Contains(BillNo))
                .AsEnumerable().OrderBy(t => t.BillNo).Select(i => new
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
                VerifyDate = i.VerifyDate == null ? string.Empty :((DateTime)i.VerifyDate).ToString("yyyy-MM-dd hh:mm:ss"),
                Status = WhatStatus(i.Status),
                IsActive = i.IsActive == "1" ? "可用" : "不可用",
                Description = i.Description,
                UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
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

        /// <summary>
        /// 生成盘点的主表ID
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
            var storages = storageQuery.OrderBy(s => s.StorageCode).AsEnumerable().Select(s => new
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

                storages = storageQuery.ToList().Where(s => ware.Contains(s.Cell.Shelf.Area.Warehouse.WarehouseCode) || area.Contains(s.Cell.Shelf.Area.AreaCode) || shelf.Contains(s.Cell.Shelf.ShelfCode) || cell.Contains(s.Cell.CellCode))
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
        /// 根据参数生成并保存盘点数据  --货位生成
        /// </summary>
        /// <param name="ware">仓库</param>
        /// <param name="area">库区</param>
        /// <param name="shelf">货架</param>
        /// <param name="cell">货位</param>
        /// <param name="UserName">登陆用户</param>
        /// <returns></returns>
        public bool CellAdd(string ware, string area, string shelf, string cell, string UserName)
        {
            bool result = false;
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.UserName == UserName);
            if (employee != null)
            {
                #region ware 这个有值，就把这个值里面所有的仓库的货位的储存信息生成盘点单，一个仓库一个盘点单据

                if (ware != null && ware != string.Empty)
                {
                    ware = ware.Substring(0, ware.Length - 1);
                    var wares = wareQuery.Where(w => ware.Contains(w.WarehouseCode));

                    foreach (var item in wares.ToArray())
                    {
                        var storages = storageQuery.Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode)
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
                            check.BillTypeCode = "1";
                            check.WarehouseCode = ware;
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
                                checkDetail.Quantity = stor.Quantity;
                                checkDetail.RealProductCode = stor.ProductCode;
                                checkDetail.RealUnitCode = stor.UnitCode;
                                checkDetail.RealQuantity = stor.Quantity;
                                checkDetail.Status = "1";
                                CheckBillDetailRepository.Add(checkDetail);
                                CheckBillDetailRepository.SaveChanges();
                            }
                            result = true;
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
                        var storages = storageQuery.ToList().Where(s => s.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode && (area.Contains(s.Cell.Shelf.Area.AreaCode) || shelf.Contains(s.Cell.Shelf.ShelfCode) || cell.Contains(s.Cell.CellCode)))
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
                            check.BillTypeCode = "1";
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
                                checkDetail.Quantity = stor.Quantity;
                                checkDetail.RealProductCode = stor.ProductCode;
                                checkDetail.RealUnitCode = stor.UnitCode;
                                checkDetail.RealQuantity = stor.Quantity;
                                checkDetail.Status = "1";
                                CheckBillDetailRepository.Add(checkDetail);
                                CheckBillDetailRepository.SaveChanges();
                            }
                            result = true;
                        }
                    }
                }

                #endregion
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

                var storages = storageQuery.ToList().Where(s => products.Contains(s.Product.ProductCode))
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
        /// 根据参数生成并保存盘点数据  --产品生成
        /// </summary>
        /// <param name="products">产品数据</param>
        /// <param name="UserName">登陆用户</param>
        /// <returns></returns>
        public bool ProductAdd(string products, string UserName)
        {
            bool result = false;
            IQueryable<Warehouse> wareQuery = WarehouseRepository.GetQueryable();
            IQueryable<Storage> storageQuery = StorageRepository.GetQueryable();
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(e => e.UserName == UserName);
            if (employee != null)
            {
                if (products != null && products != string.Empty)
                {
                    products = products.Substring(0, products.Length - 1);

                    #region products 这个有值，就把这个值里面所有的卷烟信息所在的仓库的货位的储存信息生成盘点单，一个仓库一个盘点单据

                    var warehouses = wareQuery.OrderBy(w => w.WarehouseCode);
                    foreach (var item in warehouses.ToArray())
                    {
                        var storages = storageQuery.Where(s => products.Contains(s.Product.ProductCode) && s.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode)
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
                            check.BillTypeCode = "1";
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
                                checkDetail.Quantity = stor.Quantity;
                                checkDetail.RealProductCode = stor.ProductCode;
                                checkDetail.RealUnitCode = stor.UnitCode;
                                checkDetail.RealQuantity = stor.Quantity;
                                checkDetail.Status = "1";
                                CheckBillDetailRepository.Add(checkDetail);
                                CheckBillDetailRepository.SaveChanges();
                            }
                            result = true;
                        }
                    }

                    #endregion
                }
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
            var storages = storageQuery.ToList().Where(s => inCells.Any(i => i == s.CellCode) || outCells.Any(o => o == s.CellCode) || moveInCells.Any(mi => mi == s.CellCode) || moveOutCells.Any(mo => mo == s.CellCode))
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
                                           IsActive = s.IsActive == "1" ? "可用" : "不可用",
                                           StorageTime = s.StorageTime.ToString("yyyy-MM-dd"),
                                           UpdateTime = s.UpdateTime.ToString("yyyy-MM-dd")
                                       });
            int total = storages.Count();
            storages = storages.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = storages.ToArray() };
        }

        /// <summary>
        /// 根据参数生成并保存盘点数据  --异动生成
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="UserName">登陆用户</param>
        /// <returns></returns>
        public bool ChangedAdd(string beginDate, string endDate, string UserName)
        {
            bool result = false;
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

                #region 循环所有仓库的订单，一个仓库一个盘点单据

                var warehouses = wareQuery.OrderBy(w => w.WarehouseCode);
                foreach (var item in warehouses.ToArray())
                {
                    var inCells = inAllotQuery.Where(i => i.FinishTime >= begin && i.FinishTime <= end && i.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(i => i.CellCode).Select(i => i.CellCode);
                    var outCells = outAllotQuery.Where(o => o.FinishTime >= begin && o.FinishTime <= end && o.Cell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(o => o.CellCode).Select(o => o.CellCode);
                    var moveInCells = moveBillQuery.Where(m => m.FinishTime >= begin && m.FinishTime <= end && m.InCell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(m => m.InCell.CellCode).Select(m => m.InCell.CellCode);
                    var moveOutCells = moveBillQuery.Where(m => m.FinishTime >= begin && m.FinishTime <= end && m.OutCell.Shelf.Area.Warehouse.WarehouseCode == item.WarehouseCode).OrderBy(m => m.OutCell.CellCode).Select(m => m.OutCell.CellCode);
                    var storages = storageQuery.ToList().Where(s => inCells.Any(i => i == s.CellCode) || outCells.Any(o => o == s.CellCode) || moveInCells.Any(mi => mi == s.CellCode) || moveOutCells.Any(mo => mo == s.CellCode))
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
                        check.BillTypeCode = "1";
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
                            checkDetail.Quantity = stor.Quantity;
                            checkDetail.RealProductCode = stor.ProductCode;
                            checkDetail.RealUnitCode = stor.UnitCode;
                            checkDetail.RealQuantity = stor.Quantity;
                            checkDetail.Status = "1";
                            CheckBillDetailRepository.Add(checkDetail);
                            CheckBillDetailRepository.SaveChanges();
                        }
                        result = true;
                    }
                }

                #endregion
            }
            return result;
        }

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
        public bool confirmCheck(string billNo)
        {
            bool result = false;
            var checkbm = CheckBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo);
            if (checkbm != null && checkbm.Status == "4")
            {
                checkbm.Status = "5";
                checkbm.VerifyDate = DateTime.Now;
                checkbm.UpdateTime = DateTime.Now;
                CheckBillMasterRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        #endregion
    }
}
