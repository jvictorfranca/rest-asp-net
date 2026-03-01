using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
                    var inMemorySettings = new Dictionary<string, string>
                    {
                        ["MSSQLServerSqlConnection:ConnectionString"] = _connectionString
                    };
                    config.AddInMemoryCollection(inMemorySettings!);
                });
        }
    }
}
