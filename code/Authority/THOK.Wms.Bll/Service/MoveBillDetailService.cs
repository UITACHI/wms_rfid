using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.SignalR;
using THOK.Wms.SignalR.Common;

namespace THOK.Wms.Bll.Service
{
    public class MoveBillDetailService:ServiceBase<MoveBillDetail>, IMoveBillDetailService
    {
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IUnitRepository UnitRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }
        [Dependency]
        public IProductRepository ProductRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string resultStr = "";//错误信息字符串

        #region IMoveBillDetail 成员

        /// <summary>
        /// 判断处理状态
        /// </summary>
        /// <param name="status">数据库查询出来的状态值</param>
        /// <returns></returns>
        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "0":
                    statusStr = "已录入";
                    break;
                case "1":
                    statusStr = "已审核";
                    break;
                case "2":
                    statusStr = "执行中";
                    break;
            }
            return statusStr;
        }

        /// <summary>
        /// 查询移库细单
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="rows">行数</param>
        /// <param name="BillNo">移库单号</param>
        /// <returns></returns>
        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<MoveBillDetail> MoveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
                var moveBillDetail = MoveBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).Select(i => i);
                int total = moveBillDetail.Count();
                moveBillDetail = moveBillDetail.Skip((page - 1) * rows).Take(rows);

                var temp = moveBillDetail.ToArray().AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.OutCellCode,
                    OutCellName = i.OutCell.CellName,
                    i.OutStorageCode,
                    i.InCellCode,
                    InCellName = i.InCell.CellName,
                    i.InStorageCode,
                    i.UnitCode,
                    i.Unit.UnitName,
                    RealQuantity = i.RealQuantity / i.Unit.Count,
                    OperatePersonID = i.OperatePersonID == null ? string.Empty : i.OperatePersonID.ToString(),
                    EmployeeName = i.OperatePerson == null ? string.Empty : i.OperatePerson.EmployeeName,
                    StartTime = i.StartTime == null ? null : ((DateTime)i.StartTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    FinishTime = i.FinishTime == null ? null : ((DateTime)i.FinishTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = WhatStatus(i.Status)
                });
                return new { total, rows = temp.ToArray() };
            }
            return "";
        }

        /// <summary>
        /// 新增移库细单
        /// </summary>
        /// <param name="moveBillDetail"></param>
        /// <returns></returns>
        public bool Add(MoveBillDetail moveBillDetail,out string strResult)
        {
            bool result = false;
            try
            {
                IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
                var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == moveBillDetail.ProductCode);
                var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == moveBillDetail.UnitCode);
                var storage = StorageRepository.GetQueryable().FirstOrDefault(s => s.StorageCode == moveBillDetail.InStorageCode);
                var outStorage = StorageRepository.GetQueryable().FirstOrDefault(s => s.StorageCode == moveBillDetail.OutStorageCode);
                var outCell = CellRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.OutCellCode);
                var inCell = CellRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.InCellCode);
                Storage inStorage = null;
                if (storage != null)
                {
                    inStorage = Locker.LockStorage(storage, product);
                }
                else
                {
                    inStorage = Locker.LockEmpty(inCell);
                }
                //判断移出数量是否合理
                bool isOutQuantityRight = IsQuntityRight(moveBillDetail.RealQuantity, outStorage.InFrozenQuantity, outStorage.OutFrozenQuantity, outCell.MaxQuantity, outStorage.Quantity, "out");
                //判断移入数量是否合理
                bool isInQuantityRight = IsQuntityRight(moveBillDetail.RealQuantity, inStorage.InFrozenQuantity, inStorage.OutFrozenQuantity, inCell.MaxQuantity, inStorage.Quantity, "in");
                if (Locker.LockStorage(outStorage, product) != null)
                {
                    if (inStorage != null)
                    {
                        if (isInQuantityRight && isOutQuantityRight)
                        {
                            var mbd = new MoveBillDetail();
                            mbd.BillNo = moveBillDetail.BillNo;
                            mbd.ProductCode = moveBillDetail.ProductCode;
                            mbd.OutCellCode = moveBillDetail.OutCellCode;
                            mbd.OutStorageCode = moveBillDetail.OutStorageCode;
                            mbd.InCellCode = moveBillDetail.InCellCode;
                            mbd.InStorageCode = inStorage.StorageCode;
                            mbd.UnitCode = moveBillDetail.UnitCode;
                            mbd.RealQuantity = moveBillDetail.RealQuantity * unit.Count;
                            outStorage.OutFrozenQuantity += moveBillDetail.RealQuantity * unit.Count;
                            inStorage.InFrozenQuantity += moveBillDetail.RealQuantity * unit.Count;
                            mbd.Status = "0";

                            MoveBillDetailRepository.Add(mbd);
                            MoveBillDetailRepository.SaveChanges();
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        resultStr = "移入库存加锁失败";
                    }
                }
                else
                {
                    resultStr = "移出库存加锁失败";
                }
            }
            catch (Exception ex)
            {
                resultStr = ex.ToString();
            }
            strResult = resultStr;
            return result;
        }

        /// <summary>
        /// 删除移库细单
        /// </summary>
        /// <param name="ID">移库细单ID</param>
        /// <returns></returns>
        public bool Delete(string ID,out string strResult)
        {
            bool result = false;
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
            int intID = Convert.ToInt32(ID);
            var mbd = moveBillDetailQuery.FirstOrDefault(i => i.ID == intID);
            var outStorage=StorageRepository.GetQueryable().FirstOrDefault(s=>s.StorageCode==mbd.OutStorageCode);
            var inStorage = StorageRepository.GetQueryable().FirstOrDefault(s => s.StorageCode == mbd.InStorageCode);
            var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == mbd.ProductCode);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == mbd.UnitCode);
            try
            {
                if (Locker.LockStorage(outStorage, product) != null)
                {
                    if (Locker.LockStorage(inStorage, product) != null)
                    {
                        outStorage.OutFrozenQuantity -= mbd.RealQuantity;
                        inStorage.InFrozenQuantity -= mbd.RealQuantity;
                        MoveBillDetailRepository.Delete(mbd);
                        MoveBillDetailRepository.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        resultStr = "加锁移入库存失败，当前库存已有人在操作！";
                    }
                }
                else
                {
                    resultStr = "加锁移出库存失败，当前库存已有人在操作！";
                }
            }
            catch (Exception ex)
            {
                resultStr = ex.ToString();
            }
            strResult = resultStr;
            return result;
        }

        /// <summary>
        /// 修改移库细单
        /// </summary>
        /// <param name="moveBillDetail"></param>
        /// <returns></returns>
        public bool Save(MoveBillDetail moveBillDetail,out string strResult)
        {
            bool result = false;
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
            var mbd = moveBillDetailQuery.FirstOrDefault(i => i.ID == moveBillDetail.ID && i.BillNo == moveBillDetail.BillNo);
            var unit = UnitRepository.GetQueryable().FirstOrDefault(u => u.UnitCode == moveBillDetail.UnitCode);
            var outStorage = StorageRepository.GetQueryable().FirstOrDefault(s => s.StorageCode == mbd.OutStorageCode);
            var inStorage = StorageRepository.GetQueryable().FirstOrDefault(s => s.StorageCode == mbd.InStorageCode);
            var product = ProductRepository.GetQueryable().FirstOrDefault(p => p.ProductCode == mbd.ProductCode);
            var outCell = CellRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.OutCellCode);
            var inCell = CellRepository.GetQueryable().FirstOrDefault(c => c.CellCode == moveBillDetail.InCellCode);
            //判断移出数量是否合理
            bool isOutQuantityRight = IsQuntityRight(moveBillDetail.RealQuantity, outStorage.InFrozenQuantity, outStorage.OutFrozenQuantity-mbd.RealQuantity, outCell.MaxQuantity, outStorage.Quantity, "out");
            //判断移入数量是否合理
            bool isInQuantityRight = IsQuntityRight(moveBillDetail.RealQuantity, inStorage.InFrozenQuantity-mbd.RealQuantity, inStorage.OutFrozenQuantity, inCell.MaxQuantity, inStorage.Quantity, "in");
            if (Locker.LockStorage(outStorage, product) != null)
            {
                if (Locker.LockStorage(inStorage, product) != null)
                {
                    if (isOutQuantityRight && isInQuantityRight)
                    {
                        mbd.ProductCode = moveBillDetail.ProductCode;
                        mbd.OutCellCode = moveBillDetail.OutCellCode;
                        mbd.OutStorageCode = moveBillDetail.OutStorageCode;
                        mbd.InCellCode = moveBillDetail.InCellCode;
                        mbd.InStorageCode = moveBillDetail.InStorageCode;
                        mbd.UnitCode = moveBillDetail.UnitCode;
                        mbd.RealQuantity = moveBillDetail.RealQuantity * unit.Count;
                        outStorage.OutFrozenQuantity += moveBillDetail.RealQuantity * unit.Count;
                        inStorage.InFrozenQuantity += moveBillDetail.RealQuantity * unit.Count;
                        mbd.Status = "0";
                        MoveBillDetailRepository.SaveChanges();
                        result = true;
                    }
                }
                else
                {
                    resultStr = "加锁移入库存失败，当前库存已有人在操作！";
                }

            }
            else
            {
                resultStr = "加锁移出库存失败，当前库存已有人在操作！";
            }
            strResult = resultStr;
            return result;
        }

        /// <summary>
        /// 库存加锁
        /// </summary>
        /// <param name="billNo">订单号</param>
        /// <param name="cell">货位</param>
        /// <returns></returns>
        private Storage LockStorage(string billNo, Cell cell)
        {
            try
            {
                cell.LockTag = billNo;
                CellRepository.SaveChanges();
            }
            catch (Exception)
            {
                CellRepository.Detach(cell);
                return null;
            }

            Storage storage = null;
            try
            {
                if (cell.IsSingle == "1")
                {
                    if (cell.Storages.Count == 0)
                    {
                        storage = new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            LockTag = billNo,
                            IsActive = "0",
                            StorageTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        cell.Storages.Add(storage);
                    }
                    else if (cell.Storages.Count == 1)
                    {
                        storage = cell.Storages.Single();
                        storage.LockTag = billNo;
                    }
                }
                else
                {
                    storage = cell.Storages.Where(s => s.LockTag == null || s.LockTag == string.Empty
                                                && s.Quantity == 0
                                                && s.InFrozenQuantity == 0)
                                          .FirstOrDefault();
                    if (storage != null)
                    {
                        storage.LockTag = billNo;
                    }
                    else
                    {
                        storage = new Storage()
                        {
                            StorageCode = Guid.NewGuid().ToString(),
                            CellCode = cell.CellCode,
                            IsLock = "0",
                            LockTag = billNo,
                            IsActive = "0",
                            StorageTime = DateTime.Now,
                            UpdateTime = DateTime.Now
                        };
                        cell.Storages.Add(storage);
                    }
                }
                StorageRepository.SaveChanges();
            }
            catch (Exception)
            {
                StorageRepository.Detach(storage);
                cell.Storages.Remove(storage);
                storage = null;
            }

            cell.LockTag = string.Empty;
            CellRepository.SaveChanges();

            return storage;
        }

        /// <summary>
        /// 判断移库的数量是否合理
        /// </summary>
        /// <param name="inputQuantity">用户输入的移库数量</param>
        /// <param name="inFrozenQuantity">入库冻结量</param>
        /// <param name="outFrozenQuantity">出库冻结量</param>
        /// <param name="maxQuantity">货位最大存储量</param>
        /// <param name="currentQuantity">当前数量</param>
        /// <param name="inOrOut">移出还是移入</param>
        /// <returns></returns>
        public bool IsQuntityRight(decimal inputQuantity, decimal inFrozenQuantity, decimal outFrozenQuantity, decimal maxQuantity, decimal currentQuantity,string inOrOut)
        {
            bool result = false;
            if (inOrOut=="in")
            {
                if (inputQuantity <= (maxQuantity - inFrozenQuantity - currentQuantity))
                {
                    result = true;
                }
                else
                {
                    resultStr = "入库的数量必须小于或等于[货位最大量-（当前货位库存+入库冻结量）]";
                    return result;
                }
            }
            else if (inOrOut=="out")
            {
                if (Math.Abs(inputQuantity) <= (currentQuantity - outFrozenQuantity))
                {
                    result = true;
                }
                else
                {
                    resultStr = "出库数量必须小于或等于[当前库存量-出库冻结量]";
                    return result;
                }
            }
            return result;
        }

        #endregion
    }
}
