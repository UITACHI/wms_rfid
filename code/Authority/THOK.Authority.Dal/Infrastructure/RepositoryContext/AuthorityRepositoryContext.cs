using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Dal.Interfaces.Authority;
using System.Data.Entity;

namespace THOK.Authority.Dal.Infrastructure.RepositoryContext
{
    public class AuthorityRepositoryContext : IAuthorityRepositoryContext, IRepositoryContext
    {
        private const string OBJECT_CONTEXT_KEY = "THOK.RfidWms.DBModel.Ef.AuthorizeContext,THOK.RfidWms.DBModel.Ef.dll";
        public DbSet<T> GetObjectSet<T>() 
            where T : class
        {
            return ContextManager.GetDbContext(OBJECT_CONTEXT_KEY).Set<T>();
        }

        /// <summary>
        /// Returns the active object context
        /// </summary>
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
