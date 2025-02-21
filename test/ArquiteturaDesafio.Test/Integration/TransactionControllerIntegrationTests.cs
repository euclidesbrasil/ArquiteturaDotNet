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
namespace ArquiteturaDesafio.Test.Integration
{
    public class TransactionControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private Guid _IdCreated1;
        private Guid _IdCreated2;
        public TransactionControllerIntegrationTests()
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

        private async Task saveData()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new CreateTransactionRequest()
            {
                Amount = new Core.Application.UseCases.DTOs.MoneyDTO(100.01M),
                Description = "Teste",
                Type = Core.Domain.Enum.TransactionType.Credit
            };
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response1 = await _client.PostAsync("/transaction", content);
            response1.EnsureSuccessStatusCode();
            var responseString1 = await response1.Content.ReadAsStringAsync();
            var result1 = JsonSerializer.Deserialize<CreateTransactionResponse>(responseString1, _options);
            _IdCreated1 = result1.id;
            var response2 = await _client.PostAsync("/transaction", content);
            response2.EnsureSuccessStatusCode();
            var responseString2 = await response2.Content.ReadAsStringAsync();
            var result2 = JsonSerializer.Deserialize<CreateTransactionResponse>(responseString2, _options);
            _IdCreated2 = result2.id;
        }

        [Fact]
        public async Task Test_CreateTransaction()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new CreateTransactionRequest()
            {
                Amount = new Core.Application.UseCases.DTOs.MoneyDTO(100.01M),
                Description = "Teste",
                Type = Core.Domain.Enum.TransactionType.Credit
            };
            request.UpdateDate(DateTime.UtcNow.Date);

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/transaction", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CreateTransactionResponse>(responseString, _options);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Test_UpdateTransaction()
        {
            await saveData();
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new CreateTransactionRequest()
            {
                Amount = new Core.Application.UseCases.DTOs.MoneyDTO(100.01M),
                Description = "Teste",
                Type = Core.Domain.Enum.TransactionType.Credit
            };
            request.UpdateDate(DateTime.UtcNow.Date);
            request.UpdateId(_IdCreated1);
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/Transaction?id={_IdCreated1}", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<UpdateTransactionResponse>(responseString, _options);

            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
        }

        [Fact]
        public async Task Test_GetTransactionById()
        {
            await saveData();
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync($"/Transaction?id={_IdCreated1}");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GetTransactionsByIdResponse>(responseString, _options);

            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
        }

        [Fact]
        public async Task Test_DeleteTransaction()
        {
            await saveData();
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.DeleteAsync($"/Transaction?id={_IdCreated2}");
            response.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Test_GetAllTransactions()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _client.GetAsync("/transaction");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GetTransactionsQueryResponse>(responseString, _options);

            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
        }
    }
}
