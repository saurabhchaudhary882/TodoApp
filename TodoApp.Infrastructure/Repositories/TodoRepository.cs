using TodoApp.Core.Interfaces;
using TodoApp.Core.Models;
using TodoApp.Infrastructure.Data.Configurations.Interfaces;

/// <summary>
/// Implementation of ITodoRepository using Dapper ORM with MySQL
/// </summary>
public class TodoRepository : ITodoRepository
{
    private readonly IDatabaseConnectionFactory _connectionFactory;
    private readonly IDapperWrapper _dapperWrapper;

    public TodoRepository(IDatabaseConnectionFactory connectionFactory, IDapperWrapper dapperWrapper)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                SELECT Id, Title, Description, IsCompleted, CreatedAt, UpdatedAt, DueDate
                FROM todos
                ORDER BY CreatedAt DESC";

        return await _dapperWrapper.QueryAsync<TodoItem>(connection, sql);
    }

    public async Task<TodoItem> GetByIdAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                SELECT Id, Title, Description, IsCompleted, CreatedAt, UpdatedAt, DueDate
                FROM todos
                WHERE Id = @Id";

        return await _dapperWrapper.QueryFirstOrDefaultAsync<TodoItem>(connection, sql, new { Id = id });
    }

    public async Task<TodoItem> AddAsync(TodoItem entity)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                INSERT INTO todos (Id, Title, Description, IsCompleted, CreatedAt, UpdatedAt, DueDate)
                VALUES (@Id, @Title, @Description, @IsCompleted, @CreatedAt, @UpdatedAt, @DueDate)";

        await _dapperWrapper.ExecuteAsync(connection, sql, entity);
        return entity;
    }

    public async Task<bool> UpdateAsync(TodoItem entity)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                UPDATE todos
                SET Title = @Title,
                    Description = @Description,
                    IsCompleted = @IsCompleted,
                    UpdatedAt = @UpdatedAt,
                    DueDate = @DueDate
                WHERE Id = @Id";

        var rowsAffected = await _dapperWrapper.ExecuteAsync(connection, sql, entity);
        return rowsAffected > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = "DELETE FROM todos WHERE Id = @Id";

        var rowsAffected = await _dapperWrapper.ExecuteAsync(connection, sql, new { Id = id });
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<TodoItem>> GetCompletedAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                SELECT Id, Title, Description, IsCompleted, CreatedAt, UpdatedAt, DueDate
                FROM todos
                WHERE IsCompleted = TRUE
                ORDER BY CreatedAt DESC";

        return await _dapperWrapper.QueryAsync<TodoItem>(connection, sql);
    }

    public async Task<IEnumerable<TodoItem>> GetIncompleteAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                SELECT Id, Title, Description, IsCompleted, CreatedAt, UpdatedAt, DueDate
                FROM todos
                WHERE IsCompleted = FALSE
                ORDER BY CreatedAt DESC";

        return await _dapperWrapper.QueryAsync<TodoItem>(connection, sql);
    }

    public async Task<bool> MarkAsCompletedAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                UPDATE todos
                SET IsCompleted = TRUE,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

        var rowsAffected = await _dapperWrapper.ExecuteAsync(connection, sql, new
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow
        });

        return rowsAffected > 0;
    }

    public async Task<bool> MarkAsIncompleteAsync(Guid id)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();

        const string sql = @"
                UPDATE todos
                SET IsCompleted = FALSE,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";

        var rowsAffected = await _dapperWrapper.ExecuteAsync(connection, sql, new
        {
            Id = id,
            UpdatedAt = DateTime.UtcNow
        });

        return rowsAffected > 0;
    }
}
