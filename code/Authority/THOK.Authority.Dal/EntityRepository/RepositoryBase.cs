using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Linq.Expressions;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Dal.Infrastructure;
using THOK.Authority.Dal.EntityModels;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;

namespace THOK.Authority.Dal.EntityRepository
{
    public abstract class RepositoryBase<T> : IRepository<T>
        where T: class
    {
        public RepositoryBase()
            : this(new AuthorityRepositoryContext())
        {
        }

        public RepositoryBase(IRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext ?? new AuthorityRepositoryContext();
            _objectSet = RepositoryContext.GetObjectSet<T>();
        }

        private IObjectSet<T> _objectSet;
        public IObjectSet<T> ObjectSet
        {
            get
            {
                return _objectSet;
            }
        }

        public IRepositoryContext RepositoryContext {get;set;}

        #region IRepository Members

        public void Add(T entity)
        {
            this.ObjectSet.AddObject(entity);
        }

        public void Delete(T entity)
        {
            this.ObjectSet.DeleteObject(entity);
        }

        public IList<T> GetAll()
        {
            return this.ObjectSet.ToList<T>();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).ToList<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).FirstOrDefault<T>();
        }

        public void Attach(T entity)
        {
            this.ObjectSet.Attach(entity);
        }

        public IQueryable<T> GetQueryable()
        {
            return this.ObjectSet.AsQueryable<T>();
        }

        public long Count()
        {
            return this.ObjectSet.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).LongCount<T>();
        }

        public int SaveChanges()
        {
            return RepositoryContext.SaveChanges();
        }

        public void Delete<TSub>(TSub[] tsubs)
        {
            foreach (var tsub in tsubs)
            {
                RepositoryContext.ObjectContext.DeleteObject(tsub);
            }           
        }
        #endregion
            
    }
}
