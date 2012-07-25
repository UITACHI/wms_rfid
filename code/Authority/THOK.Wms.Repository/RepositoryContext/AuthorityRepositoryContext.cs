using System.Data.Entity;
using THOK.Common.Ef.Infrastructure;
using THOK.Common.Ef.Interfaces;
using THOK.Wms.Repository.Interfaces;

namespace THOK.Wms.Repository.RepositoryContext
{
    public class AuthorityRepositoryContext : IAuthorityRepositoryContext, IRepositoryContext
    {
        private const string OBJECT_CONTEXT_KEY = "THOK.Wms.Repository.AuthorizeContext,THOK.Wms.Repository.dll";
        public DbSet<T> GetDbSet<T>() 
            where T : class
        {
            return ContextManager.GetDbContext(OBJECT_CONTEXT_KEY).Set<T>();
        }

        public DbContext DbContext
        {
            get
            {
                return ContextManager.GetDbContext(OBJECT_CONTEXT_KEY);
            }
        }

        public int SaveChanges()
        {
            return this.DbContext.SaveChanges();
        }

        public void Terminate()
        {
            ContextManager.SetRepositoryContext(null, OBJECT_CONTEXT_KEY);
        }
    }
}
