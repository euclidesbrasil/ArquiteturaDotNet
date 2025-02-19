namespace ArquiteturaDesafio.Core.Domain.Interfaces;

public interface IUnitOfWorkNoRelational
{
    Task BeginTransactionAsync();

    Task CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);

    public void Dispose();
}
