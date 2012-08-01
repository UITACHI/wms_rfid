using System;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;
using THOK.Wms.Allot.Interfaces;
using System.Linq;
using System.Collections.Generic;

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
            return new { total, rows = query.ToArray() }; 
        }
    }
}
