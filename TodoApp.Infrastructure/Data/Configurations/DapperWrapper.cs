using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Infrastructure.Data.Configurations.Interfaces;

namespace TodoApp.Infrastructure.Data.Configurations
{
    public class DapperWrapper : IDapperWrapper
    {
        public Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null)
        {
            return connection.ExecuteAsync(sql, param);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return connection.QueryAsync<T>(sql, param);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null)
        {
            return connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }
    }
}
