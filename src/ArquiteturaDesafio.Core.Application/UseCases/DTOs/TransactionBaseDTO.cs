using ArquiteturaDesafio.Core.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class TransactionBaseDTO
    {
        public TransactionType Type { get; set; }

        public MoneyDTO Amount { get; set; } = new MoneyDTO(0);

        public string Description { get; set; } = string.Empty;

        public bool Consolidated { get; private set; }
    }
}
