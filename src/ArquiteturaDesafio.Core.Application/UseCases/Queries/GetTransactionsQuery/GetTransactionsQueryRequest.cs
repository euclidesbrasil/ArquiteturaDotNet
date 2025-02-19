using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsQuery
{
    public sealed record GetTransactionsQueryRequest(int page, int size, string order, Dictionary<string, string> filters = null) : IRequest<GetTransactionsQueryResponse>;
}
