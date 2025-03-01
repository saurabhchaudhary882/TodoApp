using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TodoApp.Core.Interfaces;
using TodoApp.Infrastructure.Data.Configurations;
using TodoApp.Infrastructure.Data.Configurations.Interfaces;


namespace TodoApp.Infrastructure
{
    /// <summary>
    /// Extension methods for registering infrastructure services
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure database settings
            services.Configure<DatabaseSettings>(options =>
             configuration.GetSection("DatabaseSettings").Bind(options));

            // Register repository and database connection factory
            services.AddTransient<ITodoRepository, TodoRepository>();
            services.AddScoped<IDapperWrapper, DapperWrapper>();
            services.AddTransient<IDatabaseConnectionFactory, MySqlConnectionFactory>();

            return services;
        }
    }
}
