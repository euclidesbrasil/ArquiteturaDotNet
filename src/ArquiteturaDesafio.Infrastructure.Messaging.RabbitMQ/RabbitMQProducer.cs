using ArquiteturaDesafio.Core.Domain.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQProducer : IProducerMessage
    {
        private readonly string _hostName;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public RabbitMQProducer(string hostName)
        {
            _hostName = hostName;

            // Definição das políticas de Retry e Circuit Breaker
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
        }

        public async Task SendMessage<T>(T message, string routingKey)
        {
            await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    // Definição do servidor Rabbit MQ
                    var factory = new ConnectionFactory { HostName = _hostName };

                    // Cria uma conexão RabbitMQ usando uma factory
                    using var connection = await factory.CreateConnectionAsync();
                    // Cria um channel com sessão e model
                    using var channel = await connection.CreateChannelAsync();
                    // Declara a fila (queue) a seguir o nome e propriedades
                    await channel.QueueDeclareAsync(
                        queue: routingKey,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                    // Serializa a mensagem
                    var json = JsonSerializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(json);
                    // Põe os dados na fila : product
                    await channel.BasicPublishAsync(exchange: "", routingKey: routingKey, body: body);
                });
            });
        }
    }
}
