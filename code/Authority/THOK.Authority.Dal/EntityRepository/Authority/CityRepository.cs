using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Linq.Expressions;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Dal.EntityModels;
using System.Data.Objects;
using THOK.Authority.Dal.Infrastructure;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;
using THOK.Authority.Dal.Interfaces.Authority;

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
    }
}
