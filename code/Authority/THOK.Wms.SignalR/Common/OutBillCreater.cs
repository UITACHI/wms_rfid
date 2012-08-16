using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using Microsoft.Practices.Unity;
using THOK.Wms.Dal.Interfaces;

namespace THOK.Wms.SignalR.Common
{
    public class OutBillCreater : THOK.Wms.SignalR.Common.IOutBillCreater
    {
        [Dependency]
        public IOutBillMasterRepository OutBillMasterRepository { get; set; }
        [Dependency]
        public IOutBillDetailRepository OutBillDetailRepository { get; set; }

        [Dependency]
        public ICellRepository CellRepository { get; set; }
        [Dependency]
        public IStorageRepository StorageRepository { get; set; }
        [Dependency]
        public IStorageLocker Locker { get; set; }

        public OutBillMaster CreateOutBillMaster(string warehouseCode, string billTypeCode, string operatePersonID)
        {
            //添加移库单主单
            string billNo = CreateOutBillNo();            
            OutBillMaster outBillMaster = new OutBillMaster();
            outBillMaster.BillNo = billNo;
            outBillMaster.BillDate = DateTime.Now;
            outBillMaster.BillTypeCode = billTypeCode;
            outBillMaster.Origin = "1";
            outBillMaster.WarehouseCode = warehouseCode;
            outBillMaster.OperatePersonID = Guid.Parse(operatePersonID);
            outBillMaster.Status = "1";            ;
            outBillMaster.IsActive = "1";
            outBillMaster.UpdateTime = DateTime.Now;
            OutBillMasterRepository.Add(outBillMaster);
            return outBillMaster;
        }

        public void AddToOutBillDetail(OutBillMaster outBillMaster, Product  product,decimal price, decimal quantity)
        {
            if (quantity > 0)
            {
                Locker.LockKey = outBillMaster.BillNo;
                OutBillDetail detail = new OutBillDetail();

                detail.BillNo = outBillMaster.BillNo;
                detail.ProductCode = product.ProductCode;
                detail.UnitCode = product.UnitCode;
                detail.Price = price;
                detail.BillQuantity = quantity;
                detail.AllotQuantity = 0;
                detail.RealQuantity = 0;

                outBillMaster.OutBillDetails.Add(detail);
            }
        }

        public string CreateOutBillNo()
        {
            string billno = "";
            IQueryable<OutBillMaster> outBillMasterQuery = OutBillMasterRepository.GetQueryable();
            string sysTime = System.DateTime.Now.ToString("yyMMdd");
            var billNos = outBillMasterQuery.Where(i => i.BillNo.Contains(sysTime))
                                                  .AsEnumerable().OrderBy(i => i.BillNo)
                                                  .Select(i => i.BillNo);
            if (billNos.Count() == 0)
            {
                billno = System.DateTime.Now.ToString("yyMMdd") + "0001" + "CK";
            }
            else
            {
                string billNoStr = billNos.Last(b => b.Contains(sysTime));
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
    }
}
