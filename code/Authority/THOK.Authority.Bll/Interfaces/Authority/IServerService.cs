using THOK.Authority.Dal.EntityModels;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface IServerService : IService<Server>
    {
        object GetDetails(int page, int rows, string serverName, string description, string url, string isActive);

        bool Add(string serverName, string description, string url, bool isActive,string cityID);

        bool Delete(string serverID);

        bool Save(string serverID, string serverName, string description, string url, bool isActive,string cityID);  
    }
}
