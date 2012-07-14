using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.Dal.Interfaces;
using System.Data.Objects.DataClasses;
using THOK.Common.Ef.Interfaces;

namespace THOK.Wms.Bll.Service
{
    public abstract class ServiceBase<T> : LoggerBase, IService<T> 
    {
        public T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public void Del<TEntity>(IRepository<TEntity> context, ICollection<TEntity> entities) where TEntity : class
        {
            var arrEntities = entities.ToArray();
            foreach (var item in arrEntities)
            {
                context.Delete(item);
            }
        }
    }
}
