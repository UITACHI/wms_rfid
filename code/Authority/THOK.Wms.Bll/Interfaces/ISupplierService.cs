using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ISupplierService:IService<Supplier>
    {
        object GetDetails(int page, int rows, string SupplierCode, string SupplierName, string IsActive);

        bool Add(Supplier supplier);

        bool Delete(string SupplierCode);

        bool Save(Supplier supplier);
    }
}
