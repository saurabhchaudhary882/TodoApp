using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Core.Models;

namespace TodoApp.Core.Interfaces
{
    /// <summary>
    /// Repository interface specific to TodoItem operations
    /// </summary>
    public interface ITodoRepository : IRepository<TodoItem>
    {
        /// <summary>
        /// Get only completed todo items
        /// </summary>
        Task<IEnumerable<TodoItem>> GetCompletedAsync();

        /// <summary>
        /// Get only incomplete todo items
        /// </summary>
        Task<IEnumerable<TodoItem>> GetIncompleteAsync();

        /// <summary>
        /// Mark a todo item as completed
        /// </summary>
        Task<bool> MarkAsCompletedAsync(Guid id);

        /// <summary>
        /// Mark a todo item as incomplete
        /// </summary>
        Task<bool> MarkAsIncompleteAsync(Guid id);
    }
}
