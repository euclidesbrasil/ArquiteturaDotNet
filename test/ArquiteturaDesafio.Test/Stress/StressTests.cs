using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ArquiteturaDesafio.Test.Integration
{
    public class StressTest
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;

        public StressTest()
        {
            _client = new HttpClient(new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                PooledConnectionIdleTimeout = TimeSpan.FromSeconds(30),
                MaxConnectionsPerServer = 2000
            });
            _client.BaseAddress = new Uri("http://localhost:5000");

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

        [Fact]
        public async Task StressSimulation()
        {
            double disponibility = 0.0;
            var token = await GetAuthTokenAsync();
            Assert.NotNull(token);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var successCount = 0;
            var errorCount = 0;
            var random = new Random();
            var iteration = 0;
            try
            {
                // 30 segundos de simulação
                while (iteration < 30)
                {
                    decimal randomDecimal = 1 + (decimal)(random.NextDouble() * (1000 - 1));
                    for (var i = 1; i <= 50; i++)
                    {
                        var request = new CreateTransactionRequest
                        {
                            Amount = new Core.Application.UseCases.DTOs.MoneyDTO(randomDecimal),
                            Description = $"Teste Randomico {Guid.NewGuid()}",
                            Type = Core.Domain.Enum.TransactionType.Credit
                        };
                        request.UpdateDate(DateTime.UtcNow.Date);

                        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

                        try
                        {
                            var response = await _client.PostAsync("/transaction", content);
                            if (response.IsSuccessStatusCode)
                            {
                                System.Threading.Interlocked.Increment(ref successCount);
                            }
                            else
                            {
                                System.Threading.Interlocked.Increment(ref errorCount);
                            }
                        }
                        catch (Exception)
                        {
                            System.Threading.Interlocked.Increment(ref errorCount);
                        }
                    }

                    await Task.Delay(1000); // Espera de 1 segundo para a próxima rodada de 50 requisições
                    iteration++;
                }
                disponibility = (successCount + errorCount) * 0.95;
                Assert.True(successCount >= disponibility, $"Nem todas as requisições foram bem-sucedidas. Sucessos: {successCount} de {(successCount + errorCount)}");
            }
            catch (Exception ex)
            {
                disponibility = (successCount + errorCount) * 0.95;
                Assert.True(successCount >= disponibility, $"Nem todas as requisições foram bem-sucedidas. Sucessos: {successCount} de {(successCount + errorCount)}; ERRO: {ex.Message}");
            }
        }
    }
}
