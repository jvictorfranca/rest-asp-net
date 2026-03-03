using FluentAssertions;
using Renci.SshNet.Sftp;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

// [assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace RestASPNet.Tests.IntegrationTests.CORS
{
    [TestCaseOrderer("RestASPNet.Tests.IntegrationTests.Tools.PriorityOrder", "RestASPNet.Tests")]
    public class PersonCORSIntegrationTests : IClassFixture<SQLServerFixture>
    {

        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;

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

            _person = createdPerson;
        }

        [Fact(DisplayName = "CORS - Create person with not allowed Origin")]
        [TestPriority(2)]

        public async Task CreatePerson_WithDisallowedOrigin_ShouldNotSucceed()
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

        [Fact(DisplayName = "CORS - Get person with not allowed Origin")]
        [TestPriority(3)]

        public async Task GetPerson_WithDisallowedOrigin_ShouldNotSucceed()
        {
            // Arrange
            AddOriginHeader("http://not-localhost:3000");

            // Act
            var response = await _httpClient.GetAsync("/api/person/v1/1");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("CORS policy does not allow this origin.");
        }

        [Fact(DisplayName = "CORS - Get person with allowed Origin")]
        [TestPriority(4)]

        public async Task GetPerson_WithAllowedOrigin_ShouldSucceed()
        {
            // Arrange
            AddOriginHeader("http://localhost:3000");

            // Act
            var response = await _httpClient.GetAsync($"/api/person/v1/{_person.Id}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var found = await response.Content.ReadFromJsonAsync<PersonDTO>();
            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be(_person.FirstName);
            found.LastName.Should().Be(_person.LastName);
            found.Adress.Should().Be(_person.Adress);
        }
    }
}
