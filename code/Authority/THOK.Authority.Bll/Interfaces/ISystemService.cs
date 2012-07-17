using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Interfaces
{
    public interface ISystemService : IService<THOK.Authority.DbModel.System>
    {
        object GetDetails(int page, int rows, string systemName, string description, string status);

        bool Add(string systemName, string description, bool status);

        bool Delete(string systemId);

        bool Save(string systemId, string systemName, string description, bool status);

        object GetSystemById(string systemID);

        object GetDetails(string userName, string systemID, string cityID);
    }
}
