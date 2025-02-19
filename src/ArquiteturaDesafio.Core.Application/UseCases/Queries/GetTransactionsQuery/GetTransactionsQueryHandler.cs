using AutoMapper;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsQuery
{ 
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQueryRequest, GetTransactionsQueryResponse>
    {
        private readonly ITransactionRepository _repository;
        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(ITransactionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<GetTransactionsQueryResponse> Handle(GetTransactionsQueryRequest request, CancellationToken cancellationToken)
        {
            var transactions = await _repository.GetPagination(new PaginationQuery()
            {
                Order = request.order,
                Page = request.page,
                Size = request.size,
                Filter = request.filters
            }, cancellationToken);

            List<TransactionQueryBaseDTO> itensReturn = new List<TransactionQueryBaseDTO>();
            foreach(var item in transactions.Data)
            {
                itensReturn.Add(_mapper.Map<TransactionQueryBaseDTO>(item));
            }

            return new GetTransactionsQueryResponse()
            {
                Data = itensReturn,
                CurrentPage = transactions.CurrentPage,
                TotalItems = transactions.TotalItems,
                TotalPages = transactions.TotalPages
            };
        }
    }
}
