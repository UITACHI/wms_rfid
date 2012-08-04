using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;

namespace THOK.Wms.Allot.Service
{
    public class OutBillAllotService:ServiceBase<OutBillAllot>,IOutBillAllotService
    {
        [Dependency]
        public IOutBillAllotRepository OutBillAllotRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

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
            var allotQuery = OutBillAllotRepository.GetQueryable();
            var query = allotQuery.Where(a => a.BillNo == billNo)
                                  .OrderBy(a => a.ID)
                                  .Select(a => new
                                  {
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
            return new { total, rows = query.ToArray() }; 
        }

        public bool AllotDelete(string billNo, long id, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    var allotDetail = ibm.OutBillAllots.Single(a => a.ID == (int)id);
                    if (string.IsNullOrEmpty(allotDetail.Storage.LockTag))
                    {
                        try
                        {
                            allotDetail.OutBillDetail.AllotQuantity -= allotDetail.AllotQuantity;
                            allotDetail.Storage.InFrozenQuantity -= allotDetail.AllotQuantity;
                            allotDetail.Storage.LockTag = string.Empty;
                            ibm.OutBillAllots.Remove(allotDetail);
                            OutBillAllotRepository.Delete(allotDetail);
                            if (ibm.OutBillAllots.Count == 0)
                            {
                                ibm.Status = "2";
                                ibm.UpdateTime = DateTime.Now;
                            }
                            OutBillAllotRepository.SaveChanges();
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
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            var cell = CellRepository.GetQueryable().Single(c => c.CellCode == cellCode);
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    var allotDetail = ibm.OutBillAllots.Single(a => a.ID == (int)id);
                    if (string.IsNullOrEmpty(allotDetail.Storage.LockTag))
                    {
                        Storage storage;
                        if (allotDetail.CellCode == cellCode)
                        {
                            storage = allotDetail.Storage;
                            storage.OutFrozenQuantity -= allotDetail.AllotQuantity;
                        }
                        else
                        {
                            storage = LockStorage(billNo, cell, allotDetail.Product);
                            allotDetail.Storage.OutFrozenQuantity -= allotDetail.AllotQuantity;
                        }
                        if (storage != null)
                        {
                            decimal q1 = allotDetail.OutBillDetail.BillQuantity - allotDetail.OutBillDetail.AllotQuantity;
                            decimal q2 = allotQuantity * allotDetail.Unit.Count;
                            decimal q3 = storage.Quantity - storage.OutFrozenQuantity;
                            if (q1 >= q2 && q2<= q3)
                            {
                                try
                                {
                                    allotDetail.OutBillDetail.AllotQuantity -= allotDetail.AllotQuantity;                                    
                                    allotDetail.OutBillDetail.AllotQuantity += q2;                                    
                                    storage.OutFrozenQuantity += q2;
                                    storage.LockTag = string.Empty;
                                    allotDetail.CellCode = storage.Cell.CellCode;
                                    allotDetail.StorageCode = storage.StorageCode;
                                    allotDetail.AllotQuantity = q2;
                                    OutBillAllotRepository.SaveChanges();
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
                                strResult = "分配数量超过订单数量,或者当前储位库存量不足！";
                            }
                        }
                        else
                        {
                            strResult = "当前选择的储位不可用，其他人正在操作或没有库存！";
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
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "3");
            if (ibm != null)
            {
                if (ibm.OutBillDetails.All(b => b.BillQuantity == b.AllotQuantity)
                    && ibm.OutBillDetails.Sum(b => b.BillQuantity) == ibm.OutBillAllots.Sum(a => a.AllotQuantity))
                {
                    if (string.IsNullOrEmpty(ibm.LockTag))
                    {
                        try
                        {
                            using (var scope = new TransactionScope())
                            {                                
                                ibm.Status = "4";
                                ibm.UpdateTime = DateTime.Now;
                                OutBillMasterRepository.SaveChanges();
                                scope.Complete();
                            }
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

        public bool AllotCancel(string billNo, out string strResult)
        {
            bool result = false;
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == billNo && i.Status == "4");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    try
                    {
                        ibm.Status = "3";
                        ibm.UpdateTime = DateTime.Now;
                        OutBillMasterRepository.SaveChanges();
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

        private Storage LockStorage(string billNo, Cell cell,Product product)
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
                storage = cell.Storages.FirstOrDefault(s=>s.ProductCode == product.ProductCode && s.Quantity - s.OutFrozenQuantity > 0);
                if (storage != null)
                {
                    storage.LockTag = billNo;
                    StorageRepository.SaveChanges();
                }
            }
            catch (Exception)
            {
                StorageRepository.Detach(storage);
                storage = null;
            }

            cell.LockTag = string.Empty;
            CellRepository.SaveChanges();
            return storage;
        }
    }
}
