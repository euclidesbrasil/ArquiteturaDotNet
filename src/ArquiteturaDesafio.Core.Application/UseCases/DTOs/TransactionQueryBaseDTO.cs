using ArquiteturaDesafio.Core.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class TransactionQueryBaseDTO : TransactionBaseDTO
    {
        public Guid Id { get; internal set; }
    }
}
