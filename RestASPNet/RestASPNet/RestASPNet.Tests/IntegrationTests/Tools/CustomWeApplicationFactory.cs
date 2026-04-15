using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestASPNet.Model.Context;

namespace RestASPNet.Tests.IntegrationTests.Tools
{
    public class CustomWeApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly string _connectionString;
        public CustomWeApplicationFactory(string connectionString)
        {
            _connectionString = connectionString;
        }


        protected override void ConfigureWebHost(IWebHostBuilder builder)
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                 var testConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "appsettings.Test.json");
                    config.Sources.Clear();
                    config.AddJsonFile(testConfigPath, optional: false, reloadOnChange: true);
                });

            builder.ConfigureServices(services => 
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MSSQLContext>));
                if(descriptor != null)
                {
                    services.Remove(descriptor);
                }
                services.AddDbContext<MSSQLContext>(options =>
                {
                    options.UseSqlServer(_connectionString);
                });
            }); 
        }
    }
}
