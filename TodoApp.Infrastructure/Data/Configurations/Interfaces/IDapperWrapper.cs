using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Infrastructure.Data.Configurations.Interfaces
{
    public interface IDapperWrapper
    {
        Task<IEnumerable<T>> QueryAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<T> QueryFirstOrDefaultAsync<T>(IDbConnection connection, string sql, object param = null);
        Task<int> ExecuteAsync(IDbConnection connection, string sql, object param = null);
    }
}
