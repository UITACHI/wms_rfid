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
            return this.dbSet.Include("Cell")
                             .Include("Storage")
                             .Include("Storage.Cell")
                             .Include("Product")
                             .Include("Unit")
                             .Include("InBillDetail")
                             .AsQueryable<InBillAllot>();
        }

        public new ParallelQuery<InBillAllot> GetParallelQuery()
        {
            return this.dbSet.Include("Cell")
                             .Include("Storage")
                             .Include("Storage.Cell")
                             .Include("Product")
                             .Include("Unit")
                             .Include("InBillDetail")
                             .AsParallel<InBillAllot>();
        }
    }
}
