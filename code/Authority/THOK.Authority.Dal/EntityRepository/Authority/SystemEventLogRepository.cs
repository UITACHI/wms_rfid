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
    public class SystemEventLogRepository : RepositoryBase<SystemEventLog>, ISystemEventLogRepository
    {
        public SystemEventLogRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

        public SystemEventLogRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
