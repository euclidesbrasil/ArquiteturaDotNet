using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Enum;
using System.Threading;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;

public class CreateTransactionHandler :
       IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;

    public CreateTransactionHandler(IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository,
        IMapper mapper
        )
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<CreateTransactionResponse> Handle(CreateTransactionRequest request,
        CancellationToken cancellationToken)
    {

        var transaction = _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.Transaction>(request);
        transaction.GenerateId();
        // Validação de UserStatus
        if (!Enum.IsDefined(typeof(TransactionType), request.Type))
        {
            throw new InvalidOperationException($"Tipo inválido para Type. Value: {request.Type}");
        }

        _transactionRepository.Create(transaction);

        await _unitOfWork.Commit(cancellationToken);
        return new CreateTransactionResponse(transaction.Id);
    }
}
