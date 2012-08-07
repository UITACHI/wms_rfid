using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using THOK.Wms.SignalR;
using THOK.Wms.SignalR.Common;

namespace THOK.Wms.Allot.Service
{
    public class InBillAllotService : ServiceBase<InBillAllot>, IInBillAllotService
    {
        [Dependency]
        public IInBillAllotRepository InBillAllotRepository { get; set; }
        [Dependency]
        public IInBillMasterRepository InBillMasterRepository { get; set; }
        [Dependency]
        public IInBillDetailRepository InBillDetailRepository { get; set; }

        [Dependency]
        public IWarehouseRepository WarehouseRepository { get; set; }
        [Dependency]
        public IAreaRepository AreaRepository { get; set; }
        [Dependency]
        public IShelfRepository ShelfRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "0":
                    statusStr = "未开始";
                    break;
                case "1":
                    statusStr = "已申请";
                    break;
                case "2":
                    statusStr = "已完成";
                    break;
            }
            return statusStr;
        }

        public object Search(string billNo, int page, int rows)
        {
            var allotQuery = InBillAllotRepository.GetQueryable();
            var query = allotQuery.Where(a => a.BillNo == billNo)
                                  .OrderBy(a => a.ID)
                                  .Select(a => new { 
                                      a.ID,
                                      a.BillNo,
                                      a.ProductCode,
                                      a.Product.ProductName,
                                      a.CellCode,
                                      a.Cell.CellName,
                                      a.StorageCode,
                                      a.UnitCode,
                                      a.Unit.UnitName,
                                      AllotQuantity = a.AllotQuantity / a.Unit.Count,
                                      RealQuantity = a.RealQuantity / a.Unit.Count,
                                      a.OperatePersonID,
                                      a.StartTime,
                                      a.FinishTime,
                                      a.Status 
                                    });

            int total = query.Count();
            query = query.Skip((page - 1) * rows).Take(rows);
            var allotBill = query.ToArray();            
            return new { total, rows = query.ToArray() }; 
        }

        public bool AllotDelete(string billNo, long id, out string strResult)
        {
            bool result = false;
            var ibm = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    var allotDetail = ibm.InBillAllots.Single(a => a.ID == (int)id);
                    if (string.IsNullOrEmpty(allotDetail.Storage.LockTag))
                    {
                        try
                        {
                            allotDetail.InBillDetail.AllotQuantity -= allotDetail.AllotQuantity;
                            allotDetail.Storage.InFrozenQuantity -= allotDetail.AllotQuantity;
                            allotDetail.Storage.LockTag = string.Empty;
                            ibm.InBillAllots.Remove(allotDetail);
                            InBillAllotRepository.Delete(allotDetail);
                            if (ibm.InBillAllots.Count == 0)
                            {
                                ibm.Status = "2";
                                ibm.UpdateTime = DateTime.Now;
                            }
                            InBillAllotRepository.SaveChanges();
                            strResult = "";
                            result = true;
                        }
                        catch (Exception)
                        {
                            strResult = "当前储位或订单其他人正在操作，请稍候重试！";
                        }
                    }
                    else
                    {
                        strResult = "当前储位其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        public bool AllotEdit(string billNo, long id, string cellCode, int allotQuantity, out string strResult)
        {
            bool result = false;
            var ibm = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            var cell = CellRepository.GetQueryable().Single(c=>c.CellCode == cellCode);
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    var allotDetail = ibm.InBillAllots.Single(a => a.ID == (int)id);
                    if (string.IsNullOrEmpty(allotDetail.Storage.LockTag))
                    {
                        Storage storage;
                        if (allotDetail.CellCode == cellCode)
                        {
                            storage = allotDetail.Storage;
                        }
                        else
                        {
                            storage = LockStorage(billNo, cell);
                            if (storage != null && (storage.Quantity != 0 || storage.InFrozenQuantity != 0))
                            {
                                storage.LockTag = string.Empty;
                                StorageRepository.SaveChanges();
                                storage = null;
                            }
                        }
                        if (storage != null )
                        {
                            decimal q1 = allotDetail.InBillDetail.BillQuantity - allotDetail.InBillDetail.AllotQuantity;
                            decimal q2 = allotQuantity * allotDetail.Unit.Count;
                            if (q1 >= q2)
                            {
                                try
                                {
                                    allotDetail.InBillDetail.AllotQuantity -= allotDetail.AllotQuantity;
                                    allotDetail.Storage.InFrozenQuantity -= allotDetail.AllotQuantity;
                                    allotDetail.InBillDetail.AllotQuantity += q2;
                                    storage.ProductCode = allotDetail.ProductCode;
                                    storage.InFrozenQuantity += q2;
                                    storage.LockTag = string.Empty;
                                    allotDetail.CellCode = storage.Cell.CellCode;
                                    allotDetail.StorageCode = storage.StorageCode;
                                    allotDetail.AllotQuantity = q2;
                                    InBillAllotRepository.SaveChanges();
                                    strResult = "保存修改成功！";
                                    result = true;
                                }
                                catch (Exception)
                                {
                                    strResult = "保存修改失败，订单或储位其他人正在操作！";
                                }
                            }
                            else
                            {
                                strResult = "分配数量超过订单数量！";
                            }
                        }
                        else
                        {
                            strResult = "当前选择的储位不可用，其他人正在操作或已有库存！";
                        }
                    }
                    else
                    {
                        strResult = "当前储位其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        public bool AllotConfirm(string billNo, out string strResult)
        {
            bool result = false;
            var ibm = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (ibm.InBillDetails.All(b => b.BillQuantity == b.AllotQuantity)
                    && ibm.InBillDetails.Sum(b => b.BillQuantity) == ibm.InBillAllots.Sum(a => a.AllotQuantity))
                {
                    if (string.IsNullOrEmpty(ibm.LockTag))
                    {
                        try
                        {
                            ibm.Status = "4";
                            ibm.UpdateTime = DateTime.Now;
                            InBillMasterRepository.SaveChanges();
                            result = true;
                            strResult = "确认成功";
                        }
                        catch (Exception)
                        {
                            strResult = "当前订单其他人正在操作，请稍候重试！";
                        }

                    }
                    else
                    {
                        strResult = "当前订单其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单分配未完成或分配结果不正确！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        public bool AllotCancelConfirm(string billNo, out string strResult)
        {
            bool result = false;
            var ibm = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "4");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    try
                    {
                        ibm.Status = "3";
                        ibm.UpdateTime = DateTime.Now;
                        InBillMasterRepository.SaveChanges();
                        result = true;
                        strResult = "取消成功";
                    }
                    catch (Exception)
                    {
                        strResult = "当前订单其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已确认，或当前订单不存在！";
            }
            return result;
        }

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

        #region IInBillAllotService 成员

        /// <summary>
        /// 取消分配
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="strResult">提示信息</param>
        /// <returns></returns>
        public bool AllotCancel(string billNo, out string strResult)
        {
            bool result = false;
            var ibm = InBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            var inAllot = InBillAllotRepository.GetQueryable().Where(o => o.BillNo == ibm.BillNo);
                            try
                            {
                                foreach (var item in inAllot.ToArray())
                                {
                                    if (Locker.LockStorage(item.Storage, item.Product) != null)//锁库存
                                    {
                                        item.Storage.InFrozenQuantity -= item.AllotQuantity;
                                        item.Storage.LockTag = string.Empty;
                                        item.InBillDetail.AllotQuantity -= item.AllotQuantity;//扣除入库细单的已分配数量
                                    }
                                    InBillAllotRepository.Delete(item);
                                }
                                InBillAllotRepository.SaveChanges();
                            }
                            catch (Exception)
                            {
                                strResult = "当前货位其他人正在操作，请稍候重试！";
                                return false;
                            }

                            ibm.Status = "2";
                            ibm.UpdateTime = DateTime.Now;
                            InBillMasterRepository.SaveChanges();
                            result = true;
                            strResult = "取消成功";

                            scope.Complete();
                        }
                    }
                    catch (Exception)
                    {
                        strResult = "当前订单其他人正在操作，请稍候重试！";
                    }
                }
                else
                {
                    strResult = "当前订单其他人正在操作，请稍候重试！";
                }
            }
            else
            {
                strResult = "当前订单状态不是已分配，或当前订单不存在！";
            }
            return result;
        }

        #endregion
    }
}
