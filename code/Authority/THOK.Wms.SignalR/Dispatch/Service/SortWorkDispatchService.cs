using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.SignalR.Connection;
using THOK.Wms.SignalR.Dispatch.Interfaces;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;

namespace THOK.Wms.SignalR.Dispatch.Service
{
    public class SortWorkDispatchService : Notifier<DispatchSortWorkConnection>, ISortWorkDispatchService
    {
        [Dependency]
        public ISortOrderDispatchRepository SortOrderDispatchRepository { get; set; }
        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }
        [Dependency]
        public IMoveBillMasterRepository MoveBillMasterRepository { get; set; }
        [Dependency]
        public IMoveBillDetailRepository MoveBillDetailRepository { get; set; }
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }
        [Dependency]
        public ISortOrderRepository SortOrderRepository { get; set; }
        [Dependency]
        public ISortOrderDetailRepository SortOrderDetailRepository { get; set; }

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

        #region ISortWorkDispatchService 成员

        public void Dispatch(string workDispatchId)
        {
            IQueryable<SortOrderDispatch> sortOrderDispatchQuery = SortOrderDispatchRepository.GetQueryable();
            IQueryable<SortOrder> sortOrderQuery = SortOrderRepository.GetQueryable();
            IQueryable<SortOrderDetail> sortOrderDetailQuery = SortOrderDetailRepository.GetQueryable();
            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            IQueryable<OutBillDetail> outBillDetailQuery = OutBillDetailRepository.GetQueryable();
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            IQueryable<MoveBillDetail> moveBillDetailQuery = MoveBillDetailRepository.GetQueryable();


            workDispatchId = workDispatchId.Substring(0, workDispatchId.Length - 1);
            var sortOrderDispatch = sortOrderDispatchQuery.Where(s => workDispatchId.Contains(s.ID.ToString()))
                                                          .GroupBy(s => s.SortingLineCode)
                                                          .Select(s => new { SortingLine = s.Key, sortOrderDisp = s });
            foreach (var item in sortOrderDispatch)
            {
                foreach (var sortLine in item.sortOrderDisp)
                {
                    var sortOrder = sortOrderQuery.Where(s => s.OrderDate == sortLine.OrderDate && s.DeliverLineCode == sortLine.DeliverLineCode);
                    var sortOrderDetail = sortOrderDetailQuery.Where(s => sortOrder.Any(so => so.OrderID == s.OrderID))
                                                              .OrderBy(s => s.ProductCode).GroupBy(s => s.Product.ProductCode).Select(s => new
                                                              {
                                                                  productCode = s.Key,
                                                                  sort = s,
                                                                  quantity = s.Sum(p=>p.RealQuantity)
                                                              });
                    string outBill = GenOutBillNo("long").ToString();
                    string moveBill = GenMoveBillNo("long").ToString();
                    if (sortOrder != null)
                    {
                        var outbm = new OutBillMaster();
                        Guid emplooyye = new Guid("2c0a649d-5f44-4a33-8e83-2b6f1b5a06d8");
                        outbm.BillNo = outBill;
                        outbm.BillDate = DateTime.Now;
                        outbm.BillTypeCode = "2001";
                        outbm.WarehouseCode = "0101";
                        outbm.OperatePersonID = emplooyye;
                        outbm.Status = "1";
                        outbm.Description = "分拣作业调度生成出库单";
                        outbm.IsActive = "1";
                        outbm.UpdateTime = DateTime.Now;

                        OutBillMasterRepository.Add(outbm);
                        OutBillMasterRepository.SaveChanges();

                        foreach (var sortDetal in sortOrderDetail)
                        {
                            foreach (var sort in sortDetal.sort)
                            {
                                var ibd = new OutBillDetail();
                                ibd.BillNo = outBill;
                                ibd.ProductCode = sort.ProductCode;
                                ibd.UnitCode = sort.UnitCode;
                                ibd.Price = sort.Price;
                                ibd.BillQuantity = sortDetal.quantity * sort.Unit.Count;
                                ibd.AllotQuantity = 0;
                                ibd.RealQuantity = 0;
                                ibd.Description = "分拣产生出库细单";

                                OutBillDetailRepository.Add(ibd);
                                OutBillDetailRepository.SaveChanges();
                            }
                        }
                    }
                    var outBillMaster =outBillMasterQuery.FirstOrDefault(o=>o.BillNo==outBill);
                    var outBillDetails = outBillDetailQuery.Where(o => o.BillNo == outBill);
                    foreach (var outBillDetail in outBillDetails)
                    {
                        
                    }
                    var sortWorkDispatch = new SortWorkDispatch();
                    sortWorkDispatch.OrderDate = sortLine.OrderDate;
                    sortWorkDispatch.SortingLineCode = sortLine.SortingLineCode;
                    sortWorkDispatch.DispatchBatch = "1";
                    sortWorkDispatch.OutBillNo = "1";
                    sortWorkDispatch.MoveBillNo = "1";
                    sortWorkDispatch.DispatchStatus = "3";
                    sortWorkDispatch.IsActive = "1";
                    sortWorkDispatch.UpdateTime = DateTime.Now;
                }
            }


        }

        public object GenOutBillNo(string userName)
        {
            string billno = "";
            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            var outBillMaster = outBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).AsEnumerable().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            if (outBillMaster.Count() == 0)
            {
                billno = System.DateTime.Now.ToString("yyMMdd") + "0001" + "CK";
            }
            else
            {
                string billNoStr = outBillMaster.Last(b => b.Contains(sysTime));
                int i = Convert.ToInt32(billNoStr.ToString().Substring(6, 4));
                i++;
                string newcode = i.ToString();
                for (int j = 0; j < 4 - i.ToString().Length; j++)
                {
                    newcode = "0" + newcode;
                }
                billno = System.DateTime.Now.ToString("yyMMdd") + newcode + "CK";
            }

            return billno;
        }


        public object GenMoveBillNo(string userName)
        {
            IQueryable<MoveBillMaster> moveBillMasterQuery = MoveBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            string billNo = "";
            var employee = EmployeeRepository.GetQueryable().FirstOrDefault(i => i.UserName == userName);
            var inBillMaster = moveBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).AsEnumerable().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (inBillMaster.Count() == 0)
            {
                billNo = System.DateTime.Now.ToString("yyMMdd") + "0001" + "MO";
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
                billNo = System.DateTime.Now.ToString("yyMMdd") + newcode + "MO";
            }

            return billNo;
        }

        public void AddOutBill()
        { 
            
        }
        #endregion
    }
}
