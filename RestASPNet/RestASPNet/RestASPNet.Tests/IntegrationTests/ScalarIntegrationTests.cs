using Azure;
using FluentAssertions;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestASPNet.Tests.IntegrationTests
{
    public class ScalarIntegrationTests : IClassFixture<SQLServerFixture>
    {
        private readonly HttpClient _httpClient;

        public ScalarIntegrationTests(SQLServerFixture sqlServerFixture)
        {
            var factory = new CustomWeApplicationFactory<Program>(sqlServerFixture.ConnectionString);
            _httpClient = factory.CreateClient
                (
                new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
                {
                    BaseAddress = new Uri("http://localhost")
                }
                );
        }
        [Fact]
        public async Task Sacalar_ShouldReturnScalarUI()
        {
            // Arrange & Act
            var response = await _httpClient.GetAsync("/scalar/");
            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("<title>Asp net 2026 with Docker and Kubernetes</title>");
        }

    }
}
