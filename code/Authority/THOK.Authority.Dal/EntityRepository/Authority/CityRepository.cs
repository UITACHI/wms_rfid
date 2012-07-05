using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Linq.Expressions;
using THOK.Authority.Dal.Interfaces;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using System.Data.Objects;
using THOK.Authority.Dal.Infrastructure;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.Common;

namespace THOK.Authority.Dal.EntityRepository.Authority
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

        public CityRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

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

            this.ObjectSet.Remove(city);
        }
    }
}
