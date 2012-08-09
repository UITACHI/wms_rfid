using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.Ef.EntityRepository;
using System.Linq;

namespace THOK.Wms.Dal.EntityRepository
{
    public class InBillAllotRepository:RepositoryBase<InBillAllot>,IInBillAllotRepository
    {
        public new IQueryable<InBillAllot> GetQueryable()
        {
            return this.dbSet.Include("Storage")
                             .Include("Storage.Cell")
                             .Include("Product")
                             .Include("Unit")
                             .Include("InBillDetail")
                             .AsQueryable<InBillAllot>();
        }
    }
}
