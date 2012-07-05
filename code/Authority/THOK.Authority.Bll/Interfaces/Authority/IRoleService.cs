using THOK.RfidWms.DBModel.Ef.Models.Authority;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface IRoleService : IService<Role>
    {
        object GetDetails(int page, int rows, string roleName, string description, string status);

        bool Add(string roleName, string description, bool status);        

        bool Delete(string roleID);

        bool Save(string roleID, string roleName, string description, bool status);

        string FindRolesForFunction(string strFunctionID);

        object GetRoleUser(string roleID);

        object GetUserInfo(string roleID);

        bool DeleteRoleUser(string roleUserIdStr);

        bool AddRoleUser(string roleID, string userIDStr);
    }
}
