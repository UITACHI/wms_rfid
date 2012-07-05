using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using THOK.Authority.Dal.Interfaces;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using System.Data.Objects;
using THOK.Authority.Dal.Infrastructure;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;
using THOK.Authority.Dal.Interfaces.Authority;

namespace THOK.Authority.Dal.EntityRepository.Authority
{
    public class UserSystemRepository : RepositoryBase<UserSystem>, IUserSystemRepository
    {        
        public UserSystemRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

        public UserSystemRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
