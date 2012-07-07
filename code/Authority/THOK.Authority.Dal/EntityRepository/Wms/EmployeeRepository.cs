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
   public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
         public EmployeeRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

         public EmployeeRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
