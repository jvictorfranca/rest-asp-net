using FluentAssertions;
using Renci.SshNet.Sftp;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace RestASPNet.Tests.IntegrationTests.CORS
{
    [TestCaseOrderer("RestASPNet.Tests.IntegrationTests.Tools.PriorityOrder", "RestASPNet.Tests")]
    public class PersonCORSIntegrationTests : IClassFixture<SQLServerFixture>
    {

        private readonly HttpClient _httpClient;

        public PersonCORSIntegrationTests(SQLServerFixture sqlServerFixture)
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

        private void AddOriginHeader(string origin)
        {
            _httpClient.DefaultRequestHeaders.Remove("Origin");
            _httpClient.DefaultRequestHeaders.Add("Origin", origin);
        }

        [Fact(DisplayName = "CORS - Create person with allowed Origin")]
        [TestPriority(1)]

        public async Task CreatePerson_WithAllowedOrigin_ShouldSucceed()
        {
            // Arrange
            AddOriginHeader("http://localhost:3000");
            var request = new PersonDTO
            {
                FirstName = "CORS",
                LastName = "Test",
                Adress = "123 CORS St",
                Gender = "Male",
            };
            
            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/person/v1", request);
            // Assert
            response.EnsureSuccessStatusCode();

            var createdPerson = await response.Content.ReadFromJsonAsync<PersonDTO>();
            createdPerson.Should().NotBeNull();
            createdPerson.Id.Should().BeGreaterThan(0);
        }

        [Fact(DisplayName = "CORS - Create person with not allowed Origin")]
        [TestPriority(2)]

        public async Task CreatePerson_WithDisallowedOrigin_ShouldSucceed()
        {
            // Arrange
            AddOriginHeader("http://not-localhost:3000");
            var request = new PersonDTO
            {
                FirstName = "CORS",
                LastName = "Test",
                Adress = "123 CORS St",
                Gender = "Male",
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/person/v1", request);
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("CORS policy does not allow this origin.");
        }
    }
}
