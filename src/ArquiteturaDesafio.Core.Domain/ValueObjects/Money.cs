using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; }

        public Money(decimal amount)
        {
            if (amount < 0) throw new ArgumentException("Amount cannot be negative.");
            Amount = amount;
        }

        public Money Add(Money value) => new Money(Amount + value.Amount);
        public Money Subtract(Money value) => new Money(Amount - value.Amount);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}
