using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;
using System.Transactions;
using THOK.Wms.SignalR.Common;
using Entities.Extensions;
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

        public bool AllotCancel(string billNo, out string strResult)
        {
            Locker.LockKey = billNo;
            bool result = false;
            var ibm = InBillMasterRepository.GetQueryable()
                                            .FirstOrDefault(i => i.BillNo == billNo 
                                                && i.Status == "3");
            if (ibm != null)
            {
                if (string.IsNullOrEmpty(ibm.LockTag))
                {
                    try
                    {
                        using (var scope = new TransactionScope())
                        {
                            var inAllot = InBillAllotRepository.GetQueryable()
                                                               .Where(o => o.BillNo == ibm.BillNo)
                                                               .ToArray();

                            var storages = inAllot.Select(i => i.Storage).ToArray();

                            if (!Locker.Lock(storages))
                            {
                                strResult = "锁定储位失败，储位其他人正在操作，无法取消分配请稍候重试！";
                                return false;
                            }

                            inAllot.AsParallel().ForAll(
                                (Action<InBillAllot>)delegate(InBillAllot i)
                                {
                                    if (i.Storage.ProductCode == i.ProductCode
                                        && i.Storage.InFrozenQuantity >= i.AllotQuantity)
                                    {
                                        lock(i.InBillDetail)
                                        {
                                            i.InBillDetail.AllotQuantity -= i.AllotQuantity;
                                        }                                        
                                        i.Storage.InFrozenQuantity -= i.AllotQuantity;
                                        i.Storage.LockTag = string.Empty;
                                    }
                                    else
                                    {
                                        throw new Exception("储位的卷烟或入库冻结量与当前分配不符，信息可能被异常修改，不能取消当前入库分配！");
                                    }
                                }
                            );

                            InBillAllotRepository.SaveChanges();

                            InBillAllotRepository.GetObjectSet()
                                .DeleteEntity(i => i.BillNo == ibm.BillNo);
                            //InBillAllotRepository.GetObjectQuery()
                            //    .DeleteAll(i => i.BillNo == ibm.BillNo,null);
                            
                            ibm.Status = "2";
                            ibm.UpdateTime = DateTime.Now;
                            InBillMasterRepository.SaveChanges();
                            result = true;
                            strResult = "取消分配成功！";

                            scope.Complete();
                        }
                    }
                    catch (Exception e)
                    {
                        strResult = "取消分配失败，详情：" + e.Message;
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
                            storage = Locker.LockEmpty(cell);
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

        /// <summary>
        /// 手工分配入库单
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="id"></param>
        /// <param name="cellCode"></param>
        /// <param name="allotQuantity"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public bool AllotAdd(string billNo, long id, string cellCode, int allotQuantity, out string strResult)
        {
            throw new NotImplementedException();
        }
    }
}
