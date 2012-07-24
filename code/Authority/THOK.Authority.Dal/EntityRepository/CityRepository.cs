using System.Linq;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;
using THOK.Common;
using THOK.Common.Ef.EntityRepository;

namespace THOK.Authority.Dal.EntityRepository
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public new void Delete(City city)
        {
            Delete(city.Servers.ToArray());

            city.RoleSystems.Do(rs => rs.RoleModules.Do(rm =>
                Delete(rm.RoleFunctions.ToArray())));
            city.RoleSystems.Do(rs => Delete(rs.RoleModules.ToArray()));
            Delete(city.RoleSystems.ToArray());

            city.UserSystems.Do(us => us.UserModules.Do(um => Delete(um.UserFunctions.ToArray())));
            city.UserSystems.Do(us => Delete(us.UserModules.ToArray()));
            Delete(city.UserSystems.ToArray());

            this.dbSet.Remove(city);
        }
    }
}
