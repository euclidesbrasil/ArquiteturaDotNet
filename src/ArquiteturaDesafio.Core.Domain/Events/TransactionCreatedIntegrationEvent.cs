using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;

namespace ArquiteturaDesafio.Core.Domain.Event
{
    public class TransactionCreatedIntegrationEvent : BaseEvent<Transaction>
    {
        public TransactionCreatedIntegrationEvent(Entities.Transaction data, string message) : base(data, message)
        {
        }
    }
}

