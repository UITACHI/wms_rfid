using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using THOK.Authority.Data;

namespace THOK.Authority.Security
{
    public class RoleService : IRoleService
    {
        public RoleService()
        {
        }

        #region IRoleService 成员

        public string FindRolesForFunction(string functionId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                Guid fid = new Guid(functionId);
               
                var roles = context.Role.Where(role
                    => (role.RoleSystems.Where(roleSystem
                        => (roleSystem.RoleModules.Where(roleModule
                            => (roleModule.RoleFunctions.Where(roleFunction
                                => roleFunction.Function.FunctionID == fid)).Count() > 0)).Count() > 0)).Count() > 0);
                var rolenames = roles.Select(i => i.RoleName);

                string result = "";
                foreach (var rolename in rolenames)
                {
                    result += rolename + ",";
                }
                return result;
            }
        }

        #endregion
    }
}
