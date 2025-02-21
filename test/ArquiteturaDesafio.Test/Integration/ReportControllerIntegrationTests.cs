using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetDailyReportQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ArquiteturaDesafio.Test.Integration
{
    public class ReportControllerIntegrationTests
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;


        public ReportControllerIntegrationTests()
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
        public async Task Test_GetDailyReport_MongoDB()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var date = DateTime.UtcNow;

            // Faz a chamada ao endpoint de relatório diário do MongoDB
            var response = await _client.GetAsync($"/report/daily/mongodb/?date={date:O}");
            response.EnsureSuccessStatusCode();

            // Lê a resposta como string
            var responseString = await response.Content.ReadAsStringAsync();

            // Desserializa a resposta
            var result = JsonSerializer.Deserialize<GetDailyReportQueryResponse>(responseString, _options);

            // Valida a resposta
            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
        }

        [Fact]
        public async Task Test_GetDailyReport_Postgres()
        {
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var date = DateTime.UtcNow;

            // Faz a chamada ao endpoint de relatório diário do PostgreSQL
            var response = await _client.GetAsync($"/report/daily/postgres/?date={date:O}");
            response.EnsureSuccessStatusCode();

            // Lê a resposta como string
            var responseString = await response.Content.ReadAsStringAsync();

            // Desserializa a resposta
            var result = JsonSerializer.Deserialize<GetDailyReportQueryResponse>(responseString, _options);

            // Valida a resposta
            Assert.NotNull(result);
            // Adicione outras validações conforme necessário
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
    }
}
