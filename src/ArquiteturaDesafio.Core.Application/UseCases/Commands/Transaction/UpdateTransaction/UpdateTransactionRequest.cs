using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction
{
    public class UpdateTransactionRequest : TransactionBaseDTO, IRequest<UpdateTransactionResponse>
    {
        public Guid Id { get; internal set; }

        public void UpdateId(Guid id)
        {
            Id = id;
        }
    }
}
