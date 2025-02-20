using ArquiteturaDesafio.Core.Application.UseCases.DTOs;

namespace ArquiteturaDesafio.Core.Application.UseCases.Queries.GetDailyReportQuery
{
    public sealed record GetDailyReportQueryResponse
    {
        public DateTime Date { get; private set; }
        public MoneyDTO InitialBalance { get; private set; }
        public MoneyDTO FinalBalance { get; private set; }
        public MoneyDTO TotalCredits { get; private set; }
        public MoneyDTO TotalDebits { get; private set; }
        public int TransactionCount { get; private set; }
    }
}
