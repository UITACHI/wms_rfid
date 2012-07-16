using System.Data;
using System.Collections.Generic;
using System.Data.Entity;

namespace THOK.Common.Ef.Repositories
{
    public class EntityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext m_dbContext;

        public EntityRepository(DbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        protected virtual void OnLoaded(TEntity entity)
        {

        }

        protected DbSet<TEntity> DbSet
        {
            get { return m_dbContext.Set<TEntity>(); }
        }

        public IEnumerable<TEntity> FindAll()
        {
            return DbSet;
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
            m_dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            m_dbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            var entry = m_dbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                //DbSet.Attach(entity);
                entry.State = EntityState.Modified;
                //var entityToUpdate = FindById(entity.Id);
                //EmitMapper.ObjectMapperManager.DefaultInstance.GetMapper<TEntity, TEntity>().Map(entity, entityToUpdate);
            }
            m_dbContext.SaveChanges();
        }
    }
}
