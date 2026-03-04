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
namespace RestASPNet.Tests.IntegrationTests.Person;

[TestCaseOrderer("RestASPNet.Tests.IntegrationTests.Tools.PriorityOrder", "RestASPNet.Tests")]
public class PersonControlerJsonIntegrationTests : IClassFixture<SQLServerFixture>
{

    private readonly HttpClient _httpClient;
    private static PersonDTO? _person;

    public PersonControlerJsonIntegrationTests(SQLServerFixture sqlServerFixture)
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

    [Fact(DisplayName = "JSON - Create person with JSON")]
    [TestPriority(1)]

    public async Task CreatePerson_ShouldReturnCreatedPerson()
    {
        // Arrange
        var request = new PersonDTO
        {
            FirstName = "JSON",
            LastName = "Test",
            Adress = "123 JSON St",
            Gender = "Male",
            Enabled = true,
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/person/v1", request);
        // Assert
        response.EnsureSuccessStatusCode();

        var createdPerson = await response.Content.ReadFromJsonAsync<PersonDTO>();
        createdPerson.Should().BeEquivalentTo(request, options => options.Excluding(x => x.Id));

        _person = createdPerson;
    }

    [Fact(DisplayName = "JSON - Update person with JSON should work ")]
    [TestPriority(2)]

    public async Task Update_ShouldReturnUpdatedPerson()
    {
        // Arrange
        var request = new PersonDTO
        {
            Id = 1,
            FirstName = "JSON_Updated",
            LastName = "Test",
            Adress = "123 JSON St",
            Gender = "Male",
            Enabled = true,
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync("/api/person/v1", request);
        // Assert
        response.EnsureSuccessStatusCode();

        var updatedPerson = await response.Content.ReadFromJsonAsync<PersonDTO>();
        updatedPerson.LastName.Should().Be(request.LastName);
    }
}
