using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Data.Configurations.Interfaces
{
    /// <summary>
    /// Interface for creating database connections
    /// </summary>
    public interface IDatabaseConnectionFactory
    {
        /// <summary>
        /// Creates a new database connection
        /// </summary>
        Task<IDbConnection> CreateConnectionAsync();
    }
}
