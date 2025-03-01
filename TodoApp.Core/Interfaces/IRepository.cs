using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Core.Interfaces
{
    /// <summary>
    /// Generic repository interface to define standard operations
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Get all entities of type T
        /// </summary>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Get entity by its unique identifier
        /// </summary>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Add a new entity
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Update an existing entity
        /// </summary>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// Delete an entity by its unique identifier
        /// </summary>
        Task<bool> DeleteAsync(Guid id);
    }
}
