using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class MoneyDTO
    {
        public MoneyDTO(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; set; } = 0;
    }
}
