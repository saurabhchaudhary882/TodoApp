using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Data.Configurations.Interfaces;

namespace TodoApp.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Factory for creating MySQL database connections
    /// </summary>
    public class MySqlConnectionFactory : IDatabaseConnectionFactory
    {
        private readonly DatabaseSettings _settings;

        public MySqlConnectionFactory(IOptions<DatabaseSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            try
            {
                var connection = new MySqlConnection(_settings.ConnectionString);
                await connection.OpenAsync();
                return connection;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
