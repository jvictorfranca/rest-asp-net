
using Microsoft.EntityFrameworkCore;
using RestASPNet.Controllers.Model.Context;

namespace RestASPNet.Configurations
{
    public static class DatabaseConfig
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration["MSSQLServerSqlConnection:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string not configured");
            }
            services.AddDbContext<MSSQLContext>(options => options.UseSqlServer(connectionString));
            return services;
        }
    }
}
