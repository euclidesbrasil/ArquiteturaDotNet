using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using MediatR;
using ArquiteturaDesafio.Core.Domain.Enum;
using System.Threading;
using ArquiteturaDesafio.Core.Domain.Event;
using ArquiteturaDesafio.Core.Application.Domain;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;

public class CreateTransactionHandler :
       IRequestHandler<CreateTransactionRequest, CreateTransactionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IProducerMessage _producerMessage;
    public CreateTransactionHandler(IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository,
        IMediator mediator,
        IMapper mapper,
        IProducerMessage producerMessage

        )
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _producerMessage = producerMessage;
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
        
        // Evento de Domínio (atualiza saldo)
        await _mediator.Publish(new TransactionCreatedDomainEvent(transaction), cancellationToken);

        // Evento de Integração (envia para RabbitMQ)
        await _producerMessage.SendMessage(transaction.CreateTransactionIntegrateEvent(), "transaction.created");

        return new CreateTransactionResponse(transaction.Id);
    }
}
