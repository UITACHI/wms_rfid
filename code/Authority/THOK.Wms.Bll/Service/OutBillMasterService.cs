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
    public class OutBillMasterService:ServiceBase<OutBillMaster>,IOutBillMasterService
    {
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region IOutBillMasterService 成员

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

        public object GetDetails(int page, int rows, string BillNo, string BillDate, string OperatePersonCode, string Status, string IsActive)
        {
            IQueryable<OutBillMaster> OutBillMasterQuery = OutBillMasterRepository.GetQueryable();
            var outBillMaster = OutBillMasterQuery.Where(i => i.BillNo.Contains(BillNo) && i.Status != "6").OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { i.BillNo, BillDate = i.BillDate.ToString("yyyy-MM-dd hh:mm:ss"), i.OperatePersonID, i.WarehouseCode, i.BillTypeCode, Status = WhatStatus(i.Status), IsActive = i.IsActive == "1" ? "可用" : "不可用", Description = i.Description, UpdateTime = i.UpdateTime.ToString("yyyy-MM-dd hh:mm:ss") });
            if (!IsActive.Equals(""))
            {
                outBillMaster = outBillMaster.Where(i => i.BillNo.Contains(BillNo) && i.IsActive.Contains(IsActive) && i.Status != "6").OrderBy(i => i.BillNo).AsEnumerable().Select(i => new { i.BillNo, i.BillDate, i.OperatePersonID, i.WarehouseCode, i.BillTypeCode, Status = WhatStatus(i.Status), IsActive = i.IsActive == "1" ? "可用" : "不可用", Description = i.Description, UpdateTime = i.UpdateTime });
            }
            int total = outBillMaster.Count();
            outBillMaster = outBillMaster.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = outBillMaster.ToArray() };
        }

        public bool Add(OutBillMaster outBillMaster)
        {
            var outbm = new OutBillMaster();
            outbm.BillNo = outBillMaster.BillNo;
            outbm.BillDate = outBillMaster.BillDate;
            outbm.BillTypeCode = outBillMaster.BillTypeCode;
            outbm.WarehouseCode = outBillMaster.WarehouseCode;
            outbm.OperatePersonID = outBillMaster.OperatePersonID;
            outbm.Status = "1";
            outbm.VerifyPersonCode = outBillMaster.VerifyPersonCode;
            outbm.VerifyDate = outBillMaster.VerifyDate;
            outbm.Description = outBillMaster.Description;
            outbm.IsActive = outBillMaster.IsActive;
            outbm.UpdateTime = DateTime.Now;

            OutBillMasterRepository.Add(outbm);
            OutBillMasterRepository.SaveChanges();
            return true;
        }

        public bool Delete(string BillNo)
        {
            var ibm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == BillNo && i.Status == "1");
            OutBillMasterRepository.Delete(ibm);
            OutBillMasterRepository.SaveChanges();
            return true;
        }

        public bool Save(OutBillMaster outBillMaster)
        {
            bool result = false;
            var outbm = OutBillMasterRepository.GetQueryable().FirstOrDefault(i => i.BillNo == outBillMaster.BillNo && i.Status == "1");
            if (outbm != null)
            {
                outbm.BillDate = outBillMaster.BillDate;
                outbm.BillTypeCode = outBillMaster.BillTypeCode;
                outbm.WarehouseCode = outBillMaster.WarehouseCode;
                outbm.OperatePersonID = outBillMaster.OperatePersonID;
                outbm.Status = "1";
                outbm.VerifyPersonCode = outBillMaster.VerifyPersonCode;
                outbm.VerifyDate = outBillMaster.VerifyDate;
                outbm.Description = outBillMaster.Description;
                outbm.IsActive = outBillMaster.IsActive;
                outbm.UpdateTime = DateTime.Now;

                OutBillMasterRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        public object GenInBillNo()
        {
            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            var outBillMaster = outBillMasterQuery.Where(i => i.BillNo.Contains(sysTime)).AsEnumerable().OrderBy(i => i.BillNo).Select(i => new { i.BillNo }.BillNo);
            if (outBillMaster.Count() == 0)
            {
                return System.DateTime.Now.ToString("yyMMdd") + "0001" + "CK";
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
                return System.DateTime.Now.ToString("yyMMdd") + newcode + "CK";
            }
        }

        #endregion
    }
}
