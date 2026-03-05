using RestASPNet.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using Testcontainers.MsSql;

namespace RestASPNet.Tests.IntegrationTests.Tools
{
    public class SQLServerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; }
        public string ConnectionString => Container.GetConnectionString();

        public SQLServerFixture()
        {
            Container = new MsSqlBuilder()
                .WithPassword("YourStrong!Passw0rd")
                .Build();
            //Container.StartAsync().GetAwaiter().GetResult();
        }


        public async Task InitializeAsync()
        {
            await Container.StartAsync();
            EvolveConfig.ExcecuteMigrations(ConnectionString);

        }
        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();

        }
    }
}
