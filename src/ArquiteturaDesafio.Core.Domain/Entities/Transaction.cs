using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Event;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;

namespace ArquiteturaDesafio.Core.Domain.Entities;

public class Transaction : BaseEntity
{
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; }
    public bool Consolidated { get; private set; }

    private Transaction() { } // Para o EF Core

    public Transaction(TransactionType type, Money amount, DateTime date, string description)
    {
        Id = Guid.NewGuid();
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
        Consolidated = false;
    }

    public void UpdateTransaction(TransactionType type, Money amount, DateTime date, string description)
    {
        Type = type;
        Amount = amount;
        Date = date.ToUniversalTime();
        Description = description;
    }


    public void GenerateId()
    {
        Id = Guid.NewGuid();
    }

    public void AddTransactionCreatedEvent()
    {
        new TransactionCreatedEvent(this, $"transacao.{this.Type.ToString()}");
    }
    public void MarkAsConsolidated()
    {
        Consolidated = true;
    }
}