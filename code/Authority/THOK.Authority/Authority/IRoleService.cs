using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Authority
{
   public interface IRoleService
    {
        object GetDetails(int page, int rows);
        object GetDetails(string id);
        bool AddRole(string roleName, string memo, bool islock);
    }
}
