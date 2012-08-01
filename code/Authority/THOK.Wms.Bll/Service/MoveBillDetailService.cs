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
    public class MoveBillDetailService:ServiceBase<MoveBillDetail>, IMoveBillDetailService
    {
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IMoveBillDetail 成员

        public object GetDetails(int page, int rows, string BillNo)
        {
            if (BillNo != "" && BillNo != null)
            {
                IQueryable<MoveBillDetail> MoveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
                var moveBillDetail = MoveBillDetailQuery.Where(i => i.BillNo.Contains(BillNo)).OrderBy(i => i.BillNo).AsEnumerable().Select(i => new
                {
                    i.ID,
                    i.BillNo,
                    i.ProductCode,
                    i.Product.ProductName,
                    i.OutCellCode,
                    OutCellName=i.OutCell.CellName,
                    i.OutStorageCode,
                    i.InCellCode,
                    InCellName=i.InCell.CellName,
                    i.InStorageCode,
                    i.UnitCode,
                    i.Unit.UnitName,
                    i.RealQuantity,
                    OperatePersonID=i.OperatePersonID == null ? string.Empty : i.OperatePersonID.ToString(),
                    EmployeeName=i.OperatePerson==null?string.Empty:i.OperatePerson.EmployeeName,
                    StartTime=i.StartTime==null?null:((DateTime)i.StartTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    FinishTime=i.FinishTime==null?null:((DateTime)i.FinishTime).ToString("yyyy-MM-dd HH:mm:ss"),
                    i.Status
                });
                int total = moveBillDetail.Count();
                moveBillDetail = moveBillDetail.Skip((page - 1) * rows).Take(rows);
                return new { total, rows = moveBillDetail.ToArray() };
            }
            return "";
        }

        /// <summary>
        /// 新增移库细单
        /// </summary>
        /// <param name="moveBillDetail"></param>
        /// <returns></returns>
        public new bool Add(MoveBillDetail moveBillDetail)
        {
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
            var mbd = new MoveBillDetail();
            mbd.BillNo=moveBillDetail.BillNo;
            mbd.ProductCode=moveBillDetail.ProductCode;
            mbd.OutCellCode = moveBillDetail.OutCellCode;
            mbd.OutStorageCode = moveBillDetail.OutStorageCode;
            mbd.InCellCode = moveBillDetail.InCellCode;
            mbd.InStorageCode = moveBillDetail.InStorageCode;
            mbd.UnitCode = moveBillDetail.UnitCode;
            mbd.RealQuantity = moveBillDetail.RealQuantity*moveBillDetail.Unit.Count;

            MoveBillDetailRepository.Add(mbd);
            MoveBillDetailRepository.SaveChanges();
            return true;
        }

        public bool Delete(string ID)
        {
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();
            int intID = Convert.ToInt32(ID);
            var mbd = moveBillDetailQuery.FirstOrDefault(i => i.ID == intID);
            MoveBillDetailRepository.Delete(mbd);
            MoveBillDetailRepository.SaveChanges();
            return true;
        }

        public bool Save(MoveBillDetail moveBillDetail)
        {
            throw new NotImplementedException();
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

        #endregion
    }
}
