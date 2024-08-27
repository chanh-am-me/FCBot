using System.Collections;

namespace Infrastructure.Persistents.Repositories;

public interface IRepositoryWrapper
{
    IRepositoryBase<TEntity> Repository<TEntity>()
        where TEntity : class, new();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
public class RepositoryWrapper(ApplicationDbContext context) : IRepositoryWrapper
{
    private readonly Hashtable hashtable = [];
    private readonly ApplicationDbContext context = context;

    public IRepositoryBase<TEntity> Repository<TEntity>()
        where TEntity : class, new()
    {
        if (!hashtable.ContainsKey(typeof(TEntity)))
        {
            hashtable.Add(typeof(TEntity), Activator.CreateInstance(typeof(IRepositoryBase<TEntity>), context));
        }

        return (IRepositoryBase<TEntity>)hashtable[typeof(TEntity)]!;
    }

    public async Task BeginTransactionAsync() => await context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync() => await context.Database.CommitTransactionAsync();

    public async Task RollbackTransactionAsync() => await context.Database.RollbackTransactionAsync();
}
