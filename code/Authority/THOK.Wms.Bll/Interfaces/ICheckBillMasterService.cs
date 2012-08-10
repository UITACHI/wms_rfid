using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICheckBillMasterService : IService<CheckBillMaster>
    {
        object GetDetails(int page, int rows, string BillNo, string beginDate, string endDate, string OperatePersonCode, string Status, string IsActive);

        bool Add(string billNo,string wareCode);

        bool CellAdd(string ware, string area, string shelf, string cell, string UserName,string billType, out string info);

        bool ProductAdd(string products, string UserName, string billType, out string info);

        bool ChangedAdd(string beginDate, string endDate, string UserName, string billType, out string info);

        object GetCellDetails(int page, int rows, string ware, string area, string shelf, string cell);

        object GetProductDetails(int page, int rows, string products);

        object GetChangedCellDetails(int page, int rows, string beginDate, string endDate);

        bool Delete(string BillNo);

        bool Save(CheckBillMaster inBillMaster);

        object GetCheckBillNo();

        bool Audit(string billNo, string userName);

        bool AntiTrial(string billNo);

        bool confirmCheck(string billNo, string userName, out string errorInfo);
    }
}
