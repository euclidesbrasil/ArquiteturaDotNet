using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetUsersById;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsById
{
    public sealed record GetTransactionsByIdRequest(Guid id) : IRequest<GetTransactionsByIdResponse>;
}
