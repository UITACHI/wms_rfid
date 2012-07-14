using THOK.RfidWms.DBModel.Ef.Models.Authority;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface ISystemService : IService<THOK.RfidWms.DBModel.Ef.Models.Authority.System>
    {
        object GetDetails(int page, int rows, string systemName, string description, string status);

        bool Add(string systemName, string description, bool status);

        bool Delete(string systemId);

        bool Save(string systemId, string systemName, string description, bool status);

        object GetSystemById(string systemID);

        object GetDetails(string userName, string systemID, string cityID);
    }
}
