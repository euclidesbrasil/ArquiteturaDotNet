using ArquiteturaDesafio.Core.Domain.Common;
using Entities = ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ArquiteturaDesafio.Core.Application.Domain
{
    public class TransactionCreatedDomainEvent : INotification
    {
        public Guid Id { get; }
        public Money Amount { get; }
        public TransactionType Type { get; }
        public DateTime Date { get; }

        public TransactionCreatedDomainEvent(Entities.Transaction transaction)
        {
            Id = transaction.Id;
            Amount = transaction.Amount;
            Type = transaction.Type;
            Date = transaction.Date;
        }
    }
}
