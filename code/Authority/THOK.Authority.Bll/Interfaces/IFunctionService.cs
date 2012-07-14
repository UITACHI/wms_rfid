using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Interfaces
{
    public interface IFunctionService:IService<Function>
    {
        object GetDetails(string ModuleId);

        bool Save(string FunctionId, string FunctionName, string ControlName, string IndicateImage);

        bool Delete(string FunctionId);

        bool Add(string ModuleId, string FunctionName, string ControlName, string IndicateImage);
    }
}
