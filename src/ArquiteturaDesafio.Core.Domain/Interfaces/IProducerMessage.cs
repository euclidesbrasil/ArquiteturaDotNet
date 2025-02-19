using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq.Expressions;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IProducerMessage
{
    Task SendMessage<T>(T message, string routingKey);
}