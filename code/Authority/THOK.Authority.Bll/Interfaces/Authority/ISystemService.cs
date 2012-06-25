using THOK.Authority.Dal.EntityModels;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface ISystemService : IService<THOK.Authority.Dal.EntityModels.System>
    {
        object GetDetails(int page, int rows, string systemName, string description, string status);

        bool Add(string systemName, string description, bool status);

        bool Delete(string systemId);

        bool Save(string systemId, string systemName, string description, bool status);
    }
}
