using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
namespace THOK.Wms.Bll.Interfaces
{

      public interface IBillTypeService : IService<BillType>
        {
            object GetDetails(int page, int rows, string BillType,string BillTypeName,string BillClass,string IsActive);

            bool Add(BillType billtype);

            //bool Delete(string billtypeCode);

            //bool Save(BillType billtype);
        }
   
}
