using System;
namespace THOK.Wms.SignalR.Common
{
    public interface IOutBillCreater
    {
        void AddToOutBillDetail(THOK.Wms.DbModel.OutBillMaster outBillMaster, THOK.Wms.DbModel.Product product, decimal price, decimal quantity);
        THOK.Wms.DbModel.OutBillMaster CreateOutBillMaster(string warehouseCode, string billTypeCode, string operatePersonID);
        string CreateOutBillNo();
    }
}
