using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Authority
{
    public interface IUserService
    {
        object GetDetails(int page, int rows);
        object GetDetails(string id);
        bool AddUser(string userName, string pwd, string ChineseName, bool isLock, bool isAdmin, string loginPc, string memo);
    }
}
