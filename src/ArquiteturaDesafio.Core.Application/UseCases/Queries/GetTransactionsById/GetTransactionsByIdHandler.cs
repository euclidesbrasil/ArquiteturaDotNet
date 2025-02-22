using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsById
{
    public sealed class GetTransactionsByIdHandler : IRequestHandler<GetTransactionsByIdRequest, GetTransactionsByIdResponse>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public GetTransactionsByIdHandler(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GetTransactionsByIdResponse> Handle(GetTransactionsByIdRequest query, CancellationToken cancellationToken)
        {

            Transaction _transaction = await _repository.Get(query.id, cancellationToken);
            if (_transaction is null)
            {
                throw new InvalidOperationException($"Transação não encontrada. Id: {query.id}");
            }

            return _mapper.Map<GetTransactionsByIdResponse>(_transaction);
        }
    }
}
