using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
   public interface IJobService:IService<Job>
    {
       object GetDetails(int page, int rows, string JobCode, string JobName, string IsActive);

       bool Add(Job job);

       bool Delete(string JobId);

       bool Save(Job job);
    }
}
