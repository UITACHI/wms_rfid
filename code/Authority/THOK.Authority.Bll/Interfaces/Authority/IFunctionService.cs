using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.RfidWms.DBModel.Ef.Models.Authority;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface IFunctionService:IService<Function>
    {
        object GetDetails(string ModuleId);

        bool Save(string FunctionId, string FunctionName, string ControlName, string IndicateImage);

        bool Delete(string FunctionId);

        bool Add(string ModuleId, string FunctionName, string ControlName, string IndicateImage);
    }
}
