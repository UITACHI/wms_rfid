using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Dal.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;

namespace THOK.Authority.Dal.EntityRepository.Wms
{
    public class ShelfRepository : RepositoryBase<Shelf>, IShelfRepository
    {
         public ShelfRepository()
            : this(new AuthorityRepositoryContext())
        {
        }

         public ShelfRepository(IAuthorityRepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
    }
}
