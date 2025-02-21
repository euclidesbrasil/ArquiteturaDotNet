using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
namespace ArquiteturaDesafio.Test.Integration
{
    public class AuthControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        public AuthControllerIntegrationTests()
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

        [Fact]
        public async Task Test_Login_Endpoint()
        {
            // Cria o comando de autenticação
            var command = new AuthenticateUserRequest
            {
                Username = "admin",
                Password = "s3nh@"
            };

            // Serializa o comando
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Faz a chamada ao endpoint de login
            var response = await _client.PostAsync("/auth/login", content);
            response.EnsureSuccessStatusCode();

            // Lê a resposta como string
            var responseString = await response.Content.ReadAsStringAsync();

            // Desserializa a resposta com opções
            var responseObject = JsonSerializer.Deserialize<AuthenticateUserResult>(responseString, _options);

            // Valida a resposta
            Assert.NotNull(responseObject);
            Assert.NotNull(responseObject.Token);
        }

        [Fact]
        public async Task Test_Invalid_Login_Endpoint()
        {
            // Cria o comando de autenticação
            var command = new AuthenticateUserRequest
            {
                Username = "admin",
                Password = "s3nh@Incorreta"
            };

            // Serializa o comando
            var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

            // Faz a chamada ao endpoint de login
            var response = await _client.PostAsync("/auth/login", content);
            response.EnsureSuccessStatusCode();

            // Lê a resposta como string
            var responseString = await response.Content.ReadAsStringAsync();

            // Desserializa a resposta
            var responseObject = JsonSerializer.Deserialize<AuthenticateUserResult>(responseString);

            // Valida a resposta
            Assert.NotNull(responseObject);
            Assert.Null(responseObject.Token);
        }
    }
}
