using ArquiteturaDesafio.Core.Domain.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ.Consumer
{
    public class RabbitMQConsumer : IConsumerMessage
    {
        private readonly string _hostName;
        private readonly string _user;
        private readonly string _password;
        private readonly AsyncRetryPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public RabbitMQConsumer(string hostName, string user, string password)
        {
            _hostName = hostName;
            _user = user;
            _password = password;

            // Definição das políticas de Retry e Circuit Breaker
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));
        }

        public async Task ConsumeQueue(string queueName, Func<string, Task> processMessage, CancellationToken cancellationToken)
        {
            await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                await _retryPolicy.ExecuteAsync(async () =>
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = _hostName,
                        UserName = _user,
                        Password = _password
                    };

                    var connection = await factory.CreateConnectionAsync();
                    var channel = await connection.CreateChannelAsync();

                    await channel.QueueDeclareAsync(queue: queueName,
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);

                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body.ToArray();
                            var message = Encoding.UTF8.GetString(body);
                            Console.WriteLine($"Mensagem Recebida: {message}");

                            // Processa a mensagem
                            await processMessage(message);

                            // Confirma o processamento da mensagem (ack)
                            await channel.BasicAckAsync(ea.DeliveryTag, false);
                        }
                        catch (Exception ex)
                        {
                            // Log do erro e nega o processamento da mensagem (nack)
                            Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
                            await channel.BasicNackAsync(ea.DeliveryTag, false, true); // Reenfileira a mensagem
                        }
                    };

                    await channel.BasicConsumeAsync(queue: queueName,
                                          autoAck: false, // Desativa o autoAck para controlar manualmente
                                          consumer: consumer);

                    // Mantém o consumidor ativo até que o cancellationToken seja acionado
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000, cancellationToken);
                    }

                    // Fecha a conexão e o canal
                    await channel.CloseAsync();
                    await connection.CloseAsync();
                });
            });
        }
    }
}
