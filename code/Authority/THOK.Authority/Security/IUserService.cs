using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THOK.Authority.Security
{
    public interface IUserService
    {
        string FindUsersForFunction(string Function);

        bool ChangePassword(string userName, string password, string newPassword);

        bool ValidateUser(string userName, string password);
        bool delete(string id);
        object GetDetails(int page, int rows);

        bool ValidateUserPermission(string userName, string cityId, string systemId);

        string GetLogOnUrl(System.Security.Principal.IPrincipal iPrincipal, string cityId, string systemId, string serverId);
    }
}