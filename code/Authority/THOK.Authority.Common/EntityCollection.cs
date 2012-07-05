using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects.DataClasses;

namespace THOK.Common
{
    public static class EntityCollection
    {
        public delegate void DoFun<TEntity>(TEntity entity);

        public static void Do<TEntity>(this ICollection<TEntity> entitys, DoFun<TEntity> func) where TEntity : class
        {
            foreach (var entity in entitys)
            {
                func.Invoke(entity);
            }
        }

        public static void Del<TEntity>(this ICollection<TEntity> entitys) where TEntity : class
        {
            var entityArray = entitys.ToArray();
            foreach (var entity in entityArray)
            {
                entitys.Remove(entity);
            }
        }
    }
}
