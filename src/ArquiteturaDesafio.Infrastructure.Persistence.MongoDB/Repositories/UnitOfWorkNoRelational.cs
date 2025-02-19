using ArquiteturaDesafio.Core.Domain.Interfaces;
using MongoDB.Driver;

namespace ArquiteturaDesafio.Infrastructure.Persistence.Repositories;

public class UnitOfWorkNoRelational : IUnitOfWorkNoRelational
{
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private IClientSessionHandle _session;

    public UnitOfWorkNoRelational(IMongoClient client, string databaseName)
    {
        _client = client;
        _database = _client.GetDatabase(databaseName);
    }

    public async Task BeginTransactionAsync()
    {
        _session = await _client.StartSessionAsync();
        _session.StartTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _session.CommitTransactionAsync(cancellationToken);
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        await _session.AbortTransactionAsync(cancellationToken);
    }

    public void Dispose()
    {
        _session?.Dispose();
    }
}