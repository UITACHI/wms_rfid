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
    public class SortWorkDispatchService : ServiceBase<SortWorkDispatch>, ISortWorkDispatchService
    {
        [Dependency]
        public ISortWorkDispatchRepository SortWorkDispatchRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region ISortWorkDispatchService 成员

        public string WhatStatus(string status)
        {
            string statusStr = "";
            switch (status)
            {
                case "1":
                    statusStr = "未调度";
                    break;
                case "2":
                    statusStr = "调度中";
                    break;
                case "3":
                    statusStr = "已调度";
                    break;
            }
            return statusStr;
        }

        public object GetDetails(int page, int rows, string OrderDate, string SortingLineName, string DispatchStatus)
        {
            IQueryable<SortWorkDispatch> SortWorkDispatchQuery = SortWorkDispatchRepository.GetQueryable();
            var sortWorkDispatch = SortWorkDispatchQuery.Where(s => s.SortingLineCode == s.SortingLineCode);
            if (OrderDate != string.Empty && OrderDate != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.OrderDate.Contains(OrderDate));
            }
            if (SortingLineName != string.Empty && SortingLineName != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.SortingLine.SortingLineName.Contains(SortingLineName));
            }
            if (DispatchStatus != string.Empty && DispatchStatus != null)
            {
                sortWorkDispatch = sortWorkDispatch.Where(s => s.DispatchStatus == DispatchStatus);
            }
            var temp = sortWorkDispatch.OrderBy(b => b.SortingLineCode).AsEnumerable().Select(b => new
            {
                b.ID,
                b.OrderDate,
                b.SortingLineCode,
                b.SortingLine.SortingLineName,
                b.OutBillNo,
                b.MoveBillNo,
                b.DispatchBatch,
                DispatchStatus=WhatStatus(b.DispatchStatus),
                IsActive = b.IsActive == "1" ? "可用" : "不可用",
                UpdateTime = (string)b.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
            });

            int total = temp.Count();
            temp = temp.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = temp.ToArray() };
        }

        public new bool Add(SortWorkDispatch sortWorkDisp)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public bool Save(SortWorkDispatch sortWorkDisp)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
