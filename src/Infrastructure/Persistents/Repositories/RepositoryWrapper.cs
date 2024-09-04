using Microsoft.Extensions.DependencyInjection;
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
public class RepositoryWrapper(ApplicationDbContext context, IServiceProvider serviceProvider) : IRepositoryWrapper
{
    private readonly Hashtable Hashtable = [];

    public IRepositoryBase<TEntity> Repository<TEntity>()
        where TEntity : class, new()
    {
        if (!Hashtable.ContainsKey(typeof(TEntity)))
        {
            Hashtable.Add(typeof(TEntity), ActivatorUtilities.CreateInstance(serviceProvider, typeof(RepositoryBase<TEntity>), context));
        }

        return (IRepositoryBase<TEntity>)Hashtable[typeof(TEntity)]!;
    }

    public async Task BeginTransactionAsync() => await context.Database.BeginTransactionAsync();

    public async Task CommitTransactionAsync() => await context.Database.CommitTransactionAsync();

    public async Task RollbackTransactionAsync() => await context.Database.RollbackTransactionAsync();
}
