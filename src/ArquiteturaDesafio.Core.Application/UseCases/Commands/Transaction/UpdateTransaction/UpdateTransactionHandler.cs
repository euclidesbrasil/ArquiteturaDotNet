using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Application.Domain;
using System.Transactions;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;

public class UpdateTransactionHandler :
       IRequestHandler<UpdateTransactionRequest, UpdateTransactionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IProducerMessage _producerMessage;
    private readonly IMediator _mediator;

    public UpdateTransactionHandler(IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository,
        IMapper mapper,
        IProducerMessage producerMessage,
        IMediator mediator
        )
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _producerMessage = producerMessage;
        _mediator = mediator;
    }

    public async Task<UpdateTransactionResponse> Handle(UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {

        var transactionFromController = _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.Transaction>(request);
        var transactionForUpdate = await _transactionRepository.Get(request.Id, cancellationToken);

        if (transactionForUpdate is null)
        {
            throw new KeyNotFoundException($"Transação não encontrada. Id: {request.Id}");
        }

        TransactionType typeBefore = transactionForUpdate.Type;
        TransactionType typeAfter = transactionFromController.Type;

        decimal valueBefore = transactionForUpdate.Amount.Amount;
        decimal valueAfter = transactionFromController.Amount.Amount;

        

        // Validação de UserStatus
        if (!Enum.IsDefined(typeof(TransactionType), request.Type))
        {
            throw new KeyNotFoundException($"Tipo inválido para Type. Value: {request.Type}");
        }

        transactionForUpdate.UpdateTransaction(
            transactionFromController.Type,
            transactionFromController.Amount,
            DateTime.Now,
            request.Description);
        _transactionRepository.Update(transactionForUpdate);
        await _unitOfWork.Commit(cancellationToken);
        var commandIntegration = transactionForUpdate.UpdateTransactionIntegrateEvent(typeBefore, typeAfter, valueBefore, valueAfter);
        
        if(commandIntegration is not null)
        {
            // Evento de Domínio (atualiza saldo)
            await _mediator.Publish(new TransactionCreatedDomainEvent(commandIntegration.Data), cancellationToken);
            await _producerMessage.SendMessage(commandIntegration, "transaction.created");
        }

        return _mapper.Map<UpdateTransactionResponse>(transactionForUpdate);
    }
}
