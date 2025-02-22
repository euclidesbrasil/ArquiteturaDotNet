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

    private Transaction() { } // Para o EF Core

    public Transaction(TransactionType type, Money amount, DateTime date, string description)
    {
        Id = Guid.NewGuid();
        Type = type;
        Amount = amount;
        Date = date;
        Description = description;
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

    public TransactionCreatedIntegrationEvent CreateTransactionIntegrateEvent()
    {
        return new TransactionCreatedIntegrationEvent(this, $"Está sendo criado um evento de criação que será enviado para fila");
    }

    public TransactionCreatedIntegrationEvent DeleteTransactionIntegrateEvent()
    {
        if (this.Type == TransactionType.Credit)
        {
            // Enviar evento de crédito
            return new TransactionCreatedIntegrationEvent(new Transaction
                (TransactionType.Debit,
                this.Amount,
                this.Date, @"Evento criado o tipo contrario ao corrente, ou seja. 
                Foi criado como CRÉDITO, mas como está sendo excluido será lancçado como DEBITO para 'zerar' o saldo dessa transação."
                ), $"Está sendo criado um evento de criação que será enviado para fila, via exclusão");

        }

        if (this.Type == TransactionType.Debit)
        {
            // Enviar evento de débito
            return new TransactionCreatedIntegrationEvent(new Transaction
               (TransactionType.Credit,
               this.Amount,
               this.Date, @"Evento criado o tipo contrario ao corrente, ou seja. 
                Foi criado como DÉBITO, mas como está sendo excluido será lancçado como CRÉDITO para 'zerar' o saldo dessa transação."
               ), $"Está sendo criado um evento de criação que será enviado para fila, via exclusão");
        }

        return null;
    }

    public TransactionCreatedIntegrationEvent UpdateTransactionIntegrateEvent(TransactionType typeBefore, TransactionType typeAfter, decimal valueBefore, decimal valueAfter)
    {
        bool changeType = typeAfter != typeBefore;
        bool changeValue = valueAfter != valueBefore;
        decimal difValue = valueAfter - valueBefore;

        if (!changeType && !changeValue)
        {
            // Não houve mudança para consolidação das informações.
            return null;
        }

        // Se houve mudança de tipo, o valor é o mesmo
        if (changeType && !changeValue)
        {
            // Se houve mudança de valor, o tipo é o mesmo
            if (typeAfter == TransactionType.Credit)
            {
                // Enviar evento de crédito
                return new TransactionCreatedIntegrationEvent(new Transaction
                    (TransactionType.Credit,
                    new Money(2 * valueAfter),
                    this.Date, "Evento criado com valor duplicado, para 'limpar' o debito do mesmo lançamento e acrescentar o saldo atual"
                    ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");

            }

            if (typeAfter == TransactionType.Debit)
            {
                // Enviar evento de débito
                return new TransactionCreatedIntegrationEvent(new Transaction
                    (TransactionType.Debit,
                    new Money(2 * valueAfter),
                    this.Date, "Evento criado com valor duplicado, para 'limpar' o credito do mesmo lançamento e acrescentar o saldo atual"
                    ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
            }
        }

        if(changeValue && !changeType)
        {
            // Se houve mudança de valor, o tipo é o mesmo
            if (typeAfter == TransactionType.Credit)
            {
                if(difValue > 0)
                {
                    // Enviar evento de crédito
                    return new TransactionCreatedIntegrationEvent(new Transaction
                        (TransactionType.Credit,
                        new Money(difValue),
                        this.Date, "Lançado apenas a diferença do valor corrente"
                        ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
                }

                if (difValue < 0)
                {
                    // Enviar evento de ajustar o valor para débito
                    return new TransactionCreatedIntegrationEvent(new Transaction
                        (TransactionType.Debit,
                        new Money(Math.Abs(difValue)),
                        this.Date, "Lançado apenas a diferença do valor corrente, mas como DEBITO, poiso valor do crédito foi ajustado para menos"
                        ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
                }


            }

            if (typeAfter == TransactionType.Debit)
            {
                if (difValue > 0)
                {
                    // Enviar evento de crédito
                    return new TransactionCreatedIntegrationEvent(new Transaction
                        (TransactionType.Debit,
                        new Money(difValue),
                        this.Date, "Lançado apenas a diferença do valor corrente"
                        ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
                }

                if (difValue < 0)
                {
                    // Enviar evento de ajustar
                    return new TransactionCreatedIntegrationEvent(new Transaction
                        (TransactionType.Credit,
                        new Money(Math.Abs(difValue)),
                        this.Date, "Lançado apenas a diferença do valor corrente, mas como DEBITO, poiso valor do crédito foi ajustado para menos"
                        ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
                }
            }
        }

        // Mudou valor e tipo
        if (typeAfter == TransactionType.Credit)
        {
            // Era debito antes...
            return new TransactionCreatedIntegrationEvent(new Transaction
                        (TransactionType.Credit,
                        new Money(valueAfter + valueBefore),
                        this.Date, "Lançado o valor de debito anterior como credito, para zerar o calculo, e adicionando o atual valor"
                        ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
        }

        if (typeAfter == TransactionType.Debit)
        {
            return new TransactionCreatedIntegrationEvent(new Transaction
                        (TransactionType.Debit,
                        new Money(valueAfter + valueBefore),
                        this.Date, "Lançado o valor de credito anterior como debito, para zerar o calculo, e adicionando o atual valor"
                        ), $"Está sendo criado um evento de criação que será enviado para fila, via edicao");
        }

        return null;
    }

    public TransactionCreatedIntegrationEvent DeleteTransactionIntegrateEvent(Type typeBefore, Type typeAfter, decimal valueBefore, decimal valeuAfter)
    {
        return new TransactionCreatedIntegrationEvent(this, $"Está sendo criado um evento de criação que será enviado para fila");
    }
}