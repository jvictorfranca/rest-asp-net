using EvolveDb;
using Microsoft.Data.SqlClient;
using Serilog;

namespace RestASPNet.Configurations
{
    public static class EvolveConfig
    {
        public static IServiceCollection AddEvolveConfiguration(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                var connectionString = configuration["MSSQLServerSqlConnection:ConnectionString"];
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException("Connection string not configured");
                }

                try
                {
                    ExcecuteMigrations(connectionString);

                }
                catch (Exception ex)
                {
                    Log.Error("Database migration failed: {ErrorMessage}", ex.Message);
                    throw;
                }
            }
            return services;
        }

        public static void ExcecuteMigrations(string connectionString)
        {
            using var evolveConnection = new SqlConnection(connectionString);
            var evolve = new Evolve(evolveConnection, msg => Log.Information(msg))
            {
                Locations = new List<string> { "db/migrations", "db/dataset" },
                IsEraseDisabled = true,
            };
            evolve.Migrate();
        }
    }
}
