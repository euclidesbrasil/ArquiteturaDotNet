using ArquiteturaDesafio.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ArquiteturaDesafio.Core.Domain.ValueObjects
{
    public class Balance : ValueObject
    {
        public decimal Amount { get; }

        public Balance(decimal amount)
        {
            Amount = amount;
        }

        public Balance Add(Money value) => new Balance(Amount + value.Amount);
        public Balance Subtract(Money value) => new Balance(Amount - value.Amount);

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }
    }
}
