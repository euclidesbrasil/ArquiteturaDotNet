using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;

public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionRequest, DeleteTransactionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public DeleteTransactionHandler(IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<DeleteTransactionResponse> Handle(DeleteTransactionRequest request, CancellationToken cancellationToken)
    {
        ArquiteturaDesafio.Core.Domain.Entities.Transaction transacaoBase = await _transactionRepository.Get(request.id, cancellationToken);

        if (transacaoBase == null)
        {
            throw new KeyNotFoundException($"Transação com o ID {request.id} não existe na base de dados.");
        }

        _transactionRepository.Delete(transacaoBase);
        await _unitOfWork.Commit(cancellationToken);
        return _mapper.Map<DeleteTransactionResponse>("Transação excluida com sucesso.");
    }
}
