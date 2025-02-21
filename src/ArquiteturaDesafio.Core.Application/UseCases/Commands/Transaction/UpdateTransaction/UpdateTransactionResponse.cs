using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;

public class UpdateTransactionResponse : TransactionBaseDTO
{
    public Guid Id { get; internal set; }

    public void UpdateId(Guid id)
    {
        Id = id;
    }
}
