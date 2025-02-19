using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;
namespace ArquiteturaDesafio.Infrastructure.Persistence.Repositories;

public class BaseRepositoryNoRelational<T> : IBaseRepositoryNoRelational<T> where T : BaseEntityNoRelational
{
    protected readonly IMongoCollection<T> _collection;

    public BaseRepositoryNoRelational(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    public virtual async Task Create(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public async Task Update(T entity)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }

    public async Task Delete(T entity)
    {
        await _collection.DeleteOneAsync(x => x.Id == entity.Id);
    }
    public async Task<T> Get(int id, CancellationToken cancellationToken)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<T>> GetAll(CancellationToken cancellationToken)
    {
        return await _collection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<List<T>> Filter(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _collection.Find(predicate).ToListAsync(cancellationToken);
    }
}
