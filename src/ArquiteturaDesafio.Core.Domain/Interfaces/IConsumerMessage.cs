
namespace ArquiteturaDesafio.Core.Domain.Interfaces;
public interface IConsumerMessage
{
    Task ConsumeQueue(string queueName);
}

