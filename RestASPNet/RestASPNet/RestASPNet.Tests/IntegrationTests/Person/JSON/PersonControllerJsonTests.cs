using FluentAssertions;
using Renci.SshNet.Sftp;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Hypermedia.Utils;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace RestASPNet.Tests.IntegrationTests.Person;

[TestCaseOrderer("RestASPNet.Tests.IntegrationTests.Tools.PriorityOrder", "RestASPNet.Tests")]
public class PersonControlerJsonIntegrationTests: IClassFixture<SQLServerFixture>
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

    [Fact(DisplayName = "01 - Create person with JSON")]
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

    [Fact(DisplayName = "02 - Update person with JSON should work ")]
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

    [Fact(DisplayName = "03 - Disable person by Id with JSON should work ")]
    [TestPriority(3)]
    public async Task DisablePePersonById_ShouldReturnDisabledPerson()
    {
        // Act
        var response = await _httpClient.PatchAsync($"/api/person/v1/{_person.Id}", null);
        // Assert
        response.EnsureSuccessStatusCode();
        var disabledPerson = await response.Content.ReadFromJsonAsync<PersonDTO>();
        disabledPerson.Enabled.Should().BeFalse();
    }

    [Fact(DisplayName = "04 - Find person by Id with JSON should work ")]
    [TestPriority(4)]
    public async Task FindPePersonById_ShouldReturnDisabledPerson()
    { 
        // Act
        var response = await _httpClient.GetAsync($"/api/person/v1/{_person.Id}");
        // Assert
        response.EnsureSuccessStatusCode();
        var person = await response.Content.ReadFromJsonAsync<PersonDTO>();
        person.Should().NotBeNull();
        person.Id.Should().Be(_person.Id);
        person.FirstName.Should().Be(_person.FirstName);
        person.Enabled.Should().Be(_person.Enabled);
    }

    [Fact(DisplayName = "05 - Delete person by Id with JSON should work ")]
    [TestPriority(5)]
    public async Task DeletePePersonById_ShouldSuccess()
    { 
        // Act
        var response = await _httpClient.DeleteAsync($"/api/person/v1/{_person.Id}");
        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact(DisplayName = "06 - Find all person should word ")]
    [TestPriority(6)]
    public async Task FindAllPePersons_ShouldSuccess()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/person/v1/asc/10/1");
        // Assert
        response.EnsureSuccessStatusCode();

        var page = await response.Content.ReadFromJsonAsync<PagedSearchDTO<PersonDTO>>();
        page.Should().NotBeNull();
        page.CurrentPage.Should().Be(1);
        var list = page?.List;

        var first = list[0];

        first.FirstName.Should().NotBeNull();
        first.Gender.Should().Be("Male");
    }
}
