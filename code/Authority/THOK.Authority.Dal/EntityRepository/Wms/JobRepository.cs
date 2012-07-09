using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Dal.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;
using THOK.Authority.Dal.Interfaces.Authority;

namespace THOK.Authority.Dal.EntityRepository.Wms
{
    public class JobRepository : RepositoryBase<Job>, IJobRepository
    {
         public JobRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

         public JobRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
