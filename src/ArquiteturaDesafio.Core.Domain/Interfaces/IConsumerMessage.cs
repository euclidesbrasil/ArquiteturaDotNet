
namespace ArquiteturaDesafio.Core.Domain.Interfaces;
public interface IConsumerMessage
{
    Task ConsumeQueue(string queueName, Func<string, Task> processMessage, CancellationToken cancellationToken);
}

