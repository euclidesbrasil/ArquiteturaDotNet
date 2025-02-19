using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;

namespace ArquiteturaDesafio.Core.Domain.Entities;

public class DailyBalance : BaseEntity
{
    public DateTime Date { get; private set; }
    public Money InitialBalance { get; private set; }
    public Money FinalBalance { get; private set; }
    public Money TotalCredits { get; private set; }
    public Money TotalDebits { get; private set; }
    public int TransactionCount { get; private set; }

    private DailyBalance() { }

    public DailyBalance(DateTime date, Money initialBalance)
    {
        Id = Guid.NewGuid();
        Date = date;
        InitialBalance = initialBalance;
        FinalBalance = initialBalance;
        TotalCredits = new Money(0);
        TotalDebits = new Money(0);
        TransactionCount = 0;
    }


    public void AddTransaction(TransactionType type, Money amount)
    {
        if (type == TransactionType.Credit)
        {
            TotalCredits = TotalCredits.Add(amount);
        }

        if (type == TransactionType.Debit)
        {
            TotalDebits = TotalDebits.Add(amount);
        }

        FinalBalance = InitialBalance.Add(TotalCredits).Subtract(TotalDebits);
        TransactionCount++;
    }
}