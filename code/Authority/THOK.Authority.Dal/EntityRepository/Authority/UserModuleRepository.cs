using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Dal.EntityModels;
using System.Data.Objects;
using THOK.Authority.Dal.Infrastructure;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;
using THOK.Authority.Dal.Interfaces.Authority;

namespace THOK.Authority.Dal.EntityRepository.Authority
{
    public class UserModuleRepository : RepositoryBase<UserModule>, IUserModuleRepository
    {        
        public UserModuleRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

        public UserModuleRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
