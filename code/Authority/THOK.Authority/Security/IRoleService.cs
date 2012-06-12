using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Security
{
    public interface IRoleService
    {
        string FindRolesForFunction(string function);
    }
}
