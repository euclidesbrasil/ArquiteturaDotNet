using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq;
using ArquiteturaDesafio.Core.Domain.Entities;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace ArquiteturaDesafio.Infrastructure.Persistence.Repositories;

public class DailyBalanceReportRepository : BaseRepositoryNoRelational<DailyBalanceReport>, IDailyBalanceReportRepository
{
    public DailyBalanceReportRepository(IMongoDatabase database)
        : base(database, "DailyBalanceReport")
    {
    }

    public override async Task Create(DailyBalanceReport entity)
    {
        await base.Create(entity);
    }

    public async Task<PaginatedResult<DailyBalanceReport>> GetPaginatedResultAsync(
    Expression<Func<DailyBalanceReport, bool>> filter,
    PaginationQuery paginationQuery,
    CancellationToken cancellationToken)
    {
        var query = _collection.Find(filter);

        /*
        if (!string.IsNullOrEmpty(paginationQuery.Order))
        {
            var sortDefinition = paginationQuery.OrderAscending
                ? Builders<Cart>.Sort.Ascending(paginationQuery.Order)
                : Builders<Cart>.Sort.Descending(paginationQuery.Order);

            query = query.Sort(sortDefinition);
        }
        */
        var totalCount = await query.CountDocumentsAsync(cancellationToken);

        var items = await query
            .Skip(paginationQuery.Skip)
            .Limit(paginationQuery.Size)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<DailyBalanceReport>
        {
            Data = items,
            TotalItems = (int)totalCount,
            CurrentPage = paginationQuery.Page
        };
    }
}
