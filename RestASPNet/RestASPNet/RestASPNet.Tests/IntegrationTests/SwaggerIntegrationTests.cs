using FluentAssertions;
using RestASPNet.Tests.IntegrationTests.Tools;

namespace RestASPNet.Tests.IntegrationTests
{
    public class SwaggerIntegrationTests : IClassFixture<SQLServerFixture>
    {
        private readonly HttpClient _httpClient;

        public SwaggerIntegrationTests(SQLServerFixture sqlServerFixture)
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
        public async Task SwaggerUI_ShouldReturnSwaggerJson()
        {
            // Act
            var response = await _httpClient.GetAsync("/swagger/v1/swagger.json");
            // Assert
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            content.Should().Contain("/api/person/v1");
        }

        [Fact]
        public async Task SwaggerUI_ShouldBeAccessible()
        {
            // Arrange & Act
            var response = await _httpClient.GetAsync("/swagger-ui/index.html");

            // Assert

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            content.Should().Contain("<div id=\"swagger-ui\">");

        }
    
    }
}
