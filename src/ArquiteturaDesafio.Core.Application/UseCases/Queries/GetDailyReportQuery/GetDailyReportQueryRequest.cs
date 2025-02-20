using ArquiteturaDesafio.Core.Application.UseCases.Queries.GetTransactionsQuery;
using ArquiteturaDesafio.Core.Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetDailyReportQuery
{
    public class GetDailyReportQueryRequest : IRequest<GetDailyReportQueryResponse>
    {
        public GetDailyReportQueryRequest(DateTime? date, DatabaseType type)
        {
            Date = date;
            Type = type;
        }
        public DateTime? Date { get; set; }
        public DatabaseType Type { get; internal set; } = DatabaseType.Mongo;

        public void SetType(DatabaseType type)
        {
            Type = type;
        }
    }
}
