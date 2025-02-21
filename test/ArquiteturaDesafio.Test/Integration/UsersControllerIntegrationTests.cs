using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsById;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsQuery;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery;
using ArquiteturaDesafio.Core.Domain.Enum;
using Newtonsoft.Json.Linq;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;
namespace ArquiteturaDesafio.Test.Integration
{
    public class UsersControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private Guid _idCreated;

        public UsersControllerIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5000")
            };
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<string> GetAuthTokenAsync()
        {
            var command = new AuthenticateUserRequest
            {
                Username = "admin",
                Password = "s3nh@"
            };

            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/auth/login", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<AuthenticateUserResult>(responseString, _options);

            return responseObject?.Token;
        }

        private async Task createData()
        {
            var token = await GetAuthTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var baseUser = $"{Guid.NewGuid()}";
            var request = new CreateUserRequest
            {
                Username = $"{baseUser}",
                Email = $"{baseUser}@example.com",
                Password = "Password123",
                Firstname = "Test",
                Lastname = "User",
                Phone = "12345678",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/Users", content);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CreateUserResponse>(responseString, _options);
            _idCreated = result.id;
        }

        [Fact]
        public async Task Test_CreateUser()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var baseUser = $"{Guid.NewGuid()}";
            var request = new CreateUserRequest
            {
                Username = $"{baseUser}",
                Email = $"{baseUser}@example.com",
                Password = "Password123",
                Firstname = "Test",
                Lastname = "User",
                Phone =  "12345678",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/Users", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CreateUserResponse>(responseString, _options);

            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
        }

        [Fact]
        public async Task Test_UpdateUser()
        {
            await createData();
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var baseUser = $"{_idCreated}";
            var request = new UpdateUserRequest()
            {
                Username = $"{baseUser}",
                Email = $"{baseUser}@example.com",
                Password = "Password123",
                Firstname = "Test",
                Lastname = "User",
                Phone = "12345678",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin,
            };
            request.UpdateId(_idCreated);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/Users?id={_idCreated}", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UpdateUserResponse>(responseString, _options);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Test_GetUserById()
        {
            await createData();
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"/Users?id={_idCreated}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GetUsersByIdResponse>(responseString, _options);

            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
        }

        [Fact]
        public async Task Test_GetAllUsers()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/Users");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotNull(responseString);
        }
    }
}
