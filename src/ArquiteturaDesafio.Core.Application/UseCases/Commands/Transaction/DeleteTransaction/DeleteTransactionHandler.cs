using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;
using ArquiteturaDesafio.Core.Application.Domain;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;

public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionRequest, DeleteTransactionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IProducerMessage _producerMessage;
    private readonly IMediator _mediator;

    public DeleteTransactionHandler(IUnitOfWork unitOfWork,
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

    public async Task<DeleteTransactionResponse> Handle(DeleteTransactionRequest request, CancellationToken cancellationToken)
    {
        ArquiteturaDesafio.Core.Domain.Entities.Transaction transacaoBase = await _transactionRepository.Get(request.id, cancellationToken);

        if (transacaoBase is null)
        {
            throw new KeyNotFoundException($"Transação não encontrada. Id: {request.id}");
        }
       
        _transactionRepository.Delete(transacaoBase);
        var commandIntegration = transacaoBase.DeleteTransactionIntegrateEvent();
        await _unitOfWork.Commit(cancellationToken);

        if (commandIntegration is not null)
        {
            // Evento de Domínio (atualiza saldo)
            await _mediator.Publish(new TransactionCreatedDomainEvent(commandIntegration.Data), cancellationToken);
            await _producerMessage.SendMessage(commandIntegration, "transaction.created");
        }

        return _mapper.Map<DeleteTransactionResponse>("Transação excluida com sucesso.");
    }
}
