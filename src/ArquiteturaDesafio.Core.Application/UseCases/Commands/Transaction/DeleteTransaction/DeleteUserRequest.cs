using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;

public sealed record DeleteTransactionRequest(Guid id) : IRequest<DeleteTransactionResponse>;

