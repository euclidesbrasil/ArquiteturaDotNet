using ArquiteturaDesafio.Core.Domain.Common;
using System.Linq.Expressions;

namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IBaseRepository<T> where T : BaseEntity
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<T> Get(Guid id, CancellationToken cancellationToken);
    Task<List<T>> GetAll(CancellationToken cancellationToken);
    Task<List<T>> Filter(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken);
    Task<PaginatedResult<T>> GetPagination(PaginationQuery paginationQuery, CancellationToken cancellationToken);
}
