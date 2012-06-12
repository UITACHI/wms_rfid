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
    }
}