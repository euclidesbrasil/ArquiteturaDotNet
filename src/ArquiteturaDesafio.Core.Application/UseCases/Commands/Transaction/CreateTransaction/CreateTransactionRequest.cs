using ArquiteturaDesafio.Core.Domain.Entities;

using MediatR;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using ArquiteturaDesafio.Core.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;

public class CreateTransactionRequest : TransactionBaseDTO, IRequest<CreateTransactionResponse>
{
    public Guid Id { get; internal set; }

    public void UpdateId(Guid id)
    {
        Id = id;
    }

    public void UpdateDate(DateTime date)
    {
        this.Date = date;
    }
}
