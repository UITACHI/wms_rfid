using System;
namespace THOK.Wms.SignalR.Common
{
    public interface IMoveBillCreater
    {
        void AddToMoveBillDetail(THOK.Wms.DbModel.MoveBillMaster moveBillMaster, THOK.Wms.DbModel.Storage sourceStorage, THOK.Wms.DbModel.Storage targetStorage, decimal moveQuantity);
        bool CheckIsNeedSyncMoveBill(string warehouseCode);
        THOK.Wms.DbModel.MoveBillMaster CreateMoveBillMaster(string warehouseCode, string billTypeCode, string operatePersonID);
        string CreateMoveBillNo();
        void CreateSyncMoveBillDetail(THOK.Wms.DbModel.MoveBillMaster moveBillMaster);
        void DeleteMoveBillDetail(THOK.Wms.DbModel.MoveBillMaster moveBillMaster);
    }
}
