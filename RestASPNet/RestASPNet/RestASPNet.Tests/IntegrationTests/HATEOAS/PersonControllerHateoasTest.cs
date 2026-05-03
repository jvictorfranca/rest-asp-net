using FluentAssertions;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace RestASPNet.Tests.IntegrationTests.HATEOAS
{
    [TestCaseOrderer(TestConfigs.TestCaseOrderFullName, TestConfigs.TestCaseOrderAssembly)]
    public class PersonControllerHateoasTest : IClassFixture<SQLServerFixture>
    {
        private static TokenDTO? _token;
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;

        public PersonControllerHateoasTest(SQLServerFixture sqlServerFixture)
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

        private void AssertLinkPattern(string content, string rel)
        {
            var pattern = $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v1.*?""";
            Regex.IsMatch(content, pattern).Should().BeTrue($"Link with rel='{rel}' should exist and have valid href");
        }

        [Fact(DisplayName = "0 - Sign in user should return token")]
        [TestPriority(0)]
        public async Task SignInUser_ShouldReturnToken()
        {
            // Arrange
            var request = new UserDTO
            {
                UserName = "leandro",
                Password = "admin123"
            };
            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/signin", request);
            // Assert
            response.EnsureSuccessStatusCode();
            var token = await response.Content.ReadFromJsonAsync<TokenDTO>();
            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNull();
            token.RefreshToken.Should().NotBeNull();
            _token = token;
        }

        [Fact(DisplayName = "01 - Create person should contain HATEOAS links")]
        [TestPriority(1)]
        public async Task Test01_CreatePersonShouldContainHATEOASLinks()
        {
            // Arrange
            var request = new PersonDTO
            {
                FirstName = "HATEOAS",
                LastName = "Test",
                Adress = "123 HATEOAS St",
                Gender = "Male",
                Enabled = true,
            };

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

            // Act

            var response = await _httpClient.PostAsJsonAsync("/api/person/v1", request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            _person = await response.Content.ReadFromJsonAsync<PersonDTO>();

            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "02 - Get person by id should contain HATEOAS links")]
        [TestPriority(2)]
        public async Task Test02_GetPersonByIdShouldContainHATEOASLinks()
        {
            // Arrange

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

            var personResponse = await _httpClient.GetAsync($"/api/person/v1/1");
            personResponse.EnsureSuccessStatusCode();
            PersonDTO? person = await personResponse.Content.ReadFromJsonAsync<PersonDTO>();
            person.LastName = "Updated HATEOAS Test";
            person.Links = null;

            // Act

            var response = await _httpClient.PutAsJsonAsync("/api/person/v1", person);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "03 - DisablePersonById")]
        [TestPriority(3)]
        public async Task Test03_DisablePersonById()
        {

            // Arrange

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

            var personResponse = await _httpClient.GetAsync($"/api/person/v1/1");
            personResponse.EnsureSuccessStatusCode();
            PersonDTO? person = await personResponse.Content.ReadFromJsonAsync<PersonDTO>();
            person.Enabled = true;
            person.Links = null;

            // Act

            var responsePut = await _httpClient.PutAsJsonAsync("/api/person/v1", person);
            responsePut.EnsureSuccessStatusCode();

            var response = await _httpClient.PatchAsync($"/api/person/v1/1", null);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "04 - GetPersonById")]
        [TestPriority(4)]
        public async Task Test04_GetPersonById()
        {
            // Arrange

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);

            // Act

            var response = await _httpClient.GetAsync($"/api/person/v1/1");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "05 - GetAllPersons")]
        [TestPriority(5)]
        public async Task Test05_GetAllPersons()
        {

            // Arrange

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.AccessToken);
            // ---------------------------
            // Act
            // ---------------------------
            // Perform the HTTP GET request to retrieve all persons.
            var response = await _httpClient
                .GetAsync("api/person/v1/asc/10/1");// Ensures the response status code is 2xx.

            // Read the response content as a string.
            var content = await response.Content.ReadAsStringAsync();

            // ---------------------------
            // Assert
            // ---------------------------
            // Extract all "id" values from the response JSON using Regex.
            var idMatches = Regex.Matches(content, @"""list"":\s*\[\s*{[^}]*""id"":\s*(\d+)");
            idMatches.Count.Should().BeGreaterThan(0, "There should be at least one person");

            // Iterate through each person id found in the response.
            foreach (Match match in idMatches)
            {
                var id = match.Groups[1].Value;

                // Expected hypermedia relations (HATEOAS links).
                var expectedRels = new[] { "collection", "self", "create", "update", "patch", "delete" };

                foreach (var rel in expectedRels)
                {
                    // Build the expected regex pattern depending on the relation.
                    // For "self" and "delete", the link must contain the specific id.
                    // For others, the link points to the base endpoint.
                    var pattern = rel switch
                    {
                        "self" or "delete" or "patch" =>
                            $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v1/{id}""",
                        _ =>
                            $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/person/v1"""
                    };

                    // Assert that the link with the correct "rel" and "href" exists.
                    Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase)
                         .Should().BeTrue($"Link '{rel}' should exist for person {id}");

                    // Assert that each link also contains a "type" attribute.
                    var typePattern = $@"""rel"":\s*""{rel}"".*?""type"":\s*""[^""]+""";
                    Regex.IsMatch(content, typePattern)
                         .Should().BeTrue($"Link '{rel}' must have a type for person {id}");
                }
            }
        }

    }
}
