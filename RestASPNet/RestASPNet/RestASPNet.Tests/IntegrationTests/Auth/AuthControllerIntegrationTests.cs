using FluentAssertions;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Tests.IntegrationTests.Tools;
using System;
using System.Net.Http.Json;

namespace RestASPNet.Tests.IntegrationTests.Auth
{

    [TestCaseOrderer("RestASPNet.Tests.IntegrationTests.Tools.PriorityOrder", "RestASPNet.Tests")]
    public class AuthControllerIntegrationTests : IClassFixture<SQLServerFixture>
    {
        private readonly HttpClient _httpClient;
        private static TokenDTO? _token;
        private static AccountCredentialDTO? _createdUser;

        public AuthControllerIntegrationTests(SQLServerFixture sqlServerFixture)
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

        [Fact(DisplayName = "01 - Create user")]
        [TestPriority(1)]

        public async Task CreateUser_ShouldReturnCreatedUser()
        {
            // Arrange
            var request = new AccountCredentialDTO
            {
                UserName = "testuser",
                FullName = "Test User",
                Password = "Test@1234"
            };
            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/create", request);
            // Assert
            response.EnsureSuccessStatusCode();
            var createdUser = await response.Content.ReadFromJsonAsync<AccountCredentialDTO>();
            createdUser.Should().BeEquivalentTo(request, options => options.Excluding(x => x.Password));
            _createdUser = createdUser;
        }

        [Fact(DisplayName = "02 - Sign in user should return token")]
        [TestPriority(2)]
        public async Task SignInUser_ShouldReturnToken()
        {
            // Arrange
            var request = new UserDTO
            {
                UserName = _createdUser!.UserName,
                Password = "Test@1234"
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

        [Fact(DisplayName = "03 - Refresh token should return new token")]
        [TestPriority(3)]
        public async Task RefreshToken_ShouldReturnNewToken()
        {
            // Arrange
            var request = _token;
            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh", request);
            // Assert
            response.EnsureSuccessStatusCode();
            var newToken = await response.Content.ReadFromJsonAsync<TokenDTO>();
            newToken.Should().NotBeNull();
            newToken.AccessToken.Should().NotBeNull();
            newToken.RefreshToken.Should().NotBeNull();
            _token = newToken;
        }

        [Fact(DisplayName = "04 - Revoke token should return no content")]
        [TestPriority(4)]
        public async Task RevokeToken_ShouldReturnNoContent()
        {
            // Arrange

            var request = new UserDTO
            {
                UserName = _createdUser!.UserName,
                Password = "Test@1234"
            };
            // Act
            var responseToken = await _httpClient.PostAsJsonAsync("/api/auth/signin", request);
            responseToken.EnsureSuccessStatusCode();
            // Assert
            var token = await responseToken.Content.ReadFromJsonAsync<TokenDTO>();
            token.Should().NotBeNull();
            token!.AccessToken.Should().NotBeNullOrEmpty();
            _token = token;

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", _token.AccessToken);
            // Act
            var response = await _httpClient.PostAsync("/api/auth/revoke", null);
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "05 - Sign in with invalid credentials should return unauthorized")]
        [TestPriority(5)]
        public async Task SignInWithInvalidCredentials_ShouldReturnUnauthorized()
        {
            // Arrange
            var request = new UserDTO
            {
                UserName = "invaliduser",
                Password = "Invalid@1234"
            };
            // Act
            var response = await _httpClient.PostAsJsonAsync("/api/auth/signin", request);
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "06 - Revoke token without header should return unauthorized")]
        [TestPriority(6)]
        public async Task RevokeTokenWithoutHeader_ShouldReturnUnauthorized()
        {
            // Arrange
            _httpClient.DefaultRequestHeaders.Authorization = null;
            // Act
            var response = await _httpClient.PostAsync("/api/auth/revoke", null);
            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}