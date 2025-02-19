using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersQuery
{
    public sealed record GetUsersQueryRequest(int page, int size, string order, Dictionary<string, string> filters = null) : IRequest<GetUsersQueryResponse>;
}
