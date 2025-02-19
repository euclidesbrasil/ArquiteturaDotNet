using Ambev.Core.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.Infrastructure.Messaging.RabbitMQ;

public static class ServiceExtensions
{
    public static void ConfigureRabbitMQApp(this IServiceCollection services,
                                                IConfiguration configuration)
    {
        services.AddScoped<IProducerMessage, RabbitMQProducer>();
    }
}
