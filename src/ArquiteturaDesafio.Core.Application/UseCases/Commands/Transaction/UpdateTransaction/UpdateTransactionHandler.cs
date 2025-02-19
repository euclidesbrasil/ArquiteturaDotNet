using AutoMapper;

using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;

namespace ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;

public class UpdateTransactionHandler :
       IRequestHandler<UpdateTransactionRequest, UpdateTransactionResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private readonly IJwtTokenService _tokenService;

    public UpdateTransactionHandler(IUnitOfWork unitOfWork,
        ITransactionRepository transactionRepository,
        IMapper mapper,
        IJwtTokenService tokenService
        )
    {
        _unitOfWork = unitOfWork;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<UpdateTransactionResponse> Handle(UpdateTransactionRequest request,
        CancellationToken cancellationToken)
    {

        var transactionFromController = _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.Transaction>(request);
        var transactionForUpdate = await _transactionRepository.Get(request.Id, cancellationToken);
        if(transactionForUpdate is null)
        {
           throw new InvalidOperationException("Transaction não localizada.");
        }

        transactionForUpdate.UpdateTransaction(
            transactionFromController.Type,
            transactionFromController.Amount,
            DateTime.Now,
            request.Description);
        _transactionRepository.Update(transactionForUpdate);
        await _unitOfWork.Commit(cancellationToken);
        return _mapper.Map<UpdateTransactionResponse>(transactionForUpdate);
    }
}
