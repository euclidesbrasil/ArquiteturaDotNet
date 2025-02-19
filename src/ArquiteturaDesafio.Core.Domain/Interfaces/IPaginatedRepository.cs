using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq.Expressions;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IPaginatedRepository<T>
{
    Task<PaginatedResult<T>> GetPaginatedAsync(PaginationQuery paginationQuery, CancellationToken cancellationToken);
}
