using ArquiteturaDesafio.Core.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
namespace ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQProducer : IProducerMessage
    {
        private readonly string _hostName;
        public RabbitMQProducer(string hostName)
        {
            _hostName = hostName;
        }
        public async Task SendMessage<T>(T message, string routingKey)
        {
            // Definição do servidor Rabbit MQ
            var factory = new ConnectionFactory
            {
                HostName = _hostName
            };

            // Cria uma conexão RabbitMQ usando uma factory
            var connection = await factory.CreateConnectionAsync();
            // Cria um channel com sessão e model
            using var channel = await connection.CreateChannelAsync();
            // Declara a fila(queue) a seguir o nome e propriedades
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
        }
    }
}