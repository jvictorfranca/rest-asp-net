using FluentAssertions;
using Renci.SshNet.Sftp;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;


namespace RestASPNet.Tests.IntegrationTests.Person;

[TestCaseOrderer("RestASPNet.Tests.IntegrationTests.Tools.PriorityOrder", "RestASPNet.Tests")]
public class PersonControlerXmlIntegrationTests : IClassFixture<SQLServerFixture>
{

    private readonly HttpClient _httpClient;
    private static PersonDTO? _person;

    public PersonControlerXmlIntegrationTests(SQLServerFixture sqlServerFixture)
    {
        var factory = new CustomWeApplicationFactory<Program>(sqlServerFixture.ConnectionString);
        _httpClient = factory.CreateClient
            (
            new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost")
            }
            );
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
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
        var response = await _httpClient.PostAsync("/api/person/v1", XmlHelper.SerializeToXml(request));
        // Assert
        response.EnsureSuccessStatusCode();

        var createdPerson = await XmlHelper.DeserializeFromXmlAsync<PersonDTO>(response);
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
        var response = await _httpClient.PutAsync("/api/person/v1", XmlHelper.SerializeToXml(request));
        // Assert
        response.EnsureSuccessStatusCode();

        var updatedPerson = await XmlHelper.DeserializeFromXmlAsync<PersonDTO>(response); ;
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
        var disabledPerson = await XmlHelper.DeserializeFromXmlAsync<PersonDTO>(response);
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
        var person = await XmlHelper.DeserializeFromXmlAsync<PersonDTO>(response);
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

    [Fact(DisplayName = "05 - Find all person should word ")]
    [TestPriority(6)]
    public async Task FindAllPePersons_ShouldSuccess()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/person/v1");
        // Assert
        response.EnsureSuccessStatusCode();

        var list = await XmlHelper.DeserializeFromXmlAsync<List<PersonDTO>>(response);
        list.Should().NotBeNull();

        var first = list[0];

        first.FirstName.Should().Be("JSON_Updated");
        first.Gender.Should().Be("Male");
    }
}
