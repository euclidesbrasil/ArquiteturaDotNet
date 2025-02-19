using ArquiteturaDesafio.Core.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
namespace ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ.Consumer;
public class RabbitMQConsumer : IConsumerMessage
{
    private readonly string _hostName;
    private readonly string _user;
    private readonly string _password;
    public RabbitMQConsumer(string hostName, string user, string password)
    {
        _hostName = hostName;
        _user = user;
        _password = password;
    }
    public RabbitMQConsumer()
    {

    }
    public async Task ConsumeQueue(string queueName)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _hostName,
            UserName = _user,
            Password = _password
        };

        var _connection = await factory.CreateConnectionAsync();
        var _channel = await _connection.CreateChannelAsync();

       await _channel.QueueDeclareAsync(queue: queueName,
                              durable: false,
                              exclusive: false,
                              autoDelete: false,
                              arguments: null);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Mensagem Recebida: {message}");
        };

        await _channel.BasicConsumeAsync(queue: queueName,
                              autoAck: true,
                              consumer: consumer);
    }
}
