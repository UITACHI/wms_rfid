using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace THOK.Authority.Dal.Interfaces
{
    /// <summary>
    /// Context across all repositories
    /// </summary>
    public interface IRepositoryContext
    {

        IObjectSet<T> GetObjectSet<T>() where T : class;

        ObjectContext ObjectContext { get; }

        /// <summary>
        /// Save all changes to all repositories
        /// </summary>
        /// <returns>Integer with number of objects affected</returns>
        int SaveChanges();

        /// <summary>
        /// Terminates the current repository context
        /// </summary>
        void Terminate();
    }
}
