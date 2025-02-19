using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Domain.Event
{
    public class TransactionCreatedIntegrationEvent : BaseEvent<Entities.Transaction>
    {
        public TransactionCreatedIntegrationEvent(Entities.Transaction data, string message) : base(data, message)
        {

        }
    }
}

