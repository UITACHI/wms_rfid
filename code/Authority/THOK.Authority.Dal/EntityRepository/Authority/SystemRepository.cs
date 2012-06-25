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
    public class SystemRepository : RepositoryBase<THOK.Authority.Dal.EntityModels.System>, ISystemRepository
    {
        public SystemRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

        public SystemRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
