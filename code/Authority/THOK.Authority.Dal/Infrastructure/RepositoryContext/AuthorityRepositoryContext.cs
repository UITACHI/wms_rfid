using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Dal.Interfaces.Authority;

namespace THOK.Authority.Dal.Infrastructure.RepositoryContext
{
    public class AuthorityRepositoryContext : IAuthorityRepositoryContext, IRepositoryContext
    {
        private const string OBJECT_CONTEXT_KEY = "THOK.Authority.Dal.EntityModels.AuthorizeEntities";
        public IObjectSet<T> GetObjectSet<T>() 
            where T : class
        {
            return ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY).CreateObjectSet<T>();
        }

        /// <summary>
        /// Returns the active object context
        /// </summary>
        public ObjectContext ObjectContext
        {
            get
            {
                return ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY);
            }
        }

        public int SaveChanges()
        {
            return this.ObjectContext.SaveChanges();
        }

        public void Terminate()
        {
            ContextManager.SetRepositoryContext(null, OBJECT_CONTEXT_KEY);
        }
    }
}
