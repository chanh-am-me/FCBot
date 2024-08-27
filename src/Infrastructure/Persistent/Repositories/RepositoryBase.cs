using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Infrastructure.Persistent.Repositories;

public interface IRepositoryBase<TEntity>
    where TEntity : class, new()
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    IQueryable<TEntity> Find(Expression<Func<TEntity, bool>>? predicate = default, CancellationToken cancellationToken = default);
}

public class RepositoryBase<TEntity>(ApplicationDbContext context) : IRepositoryBase<TEntity>
    where TEntity : class, new()
{
    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        EntityEntry<TEntity> entry = await context.Set<TEntity>().AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        EntityEntry<TEntity> entry = context.Set<TEntity>().Remove(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<IEnumerable<TEntity>> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().RemoveRange(entities);
        await context.SaveChangesAsync(cancellationToken);
        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        EntityEntry<TEntity> entry = context.Set<TEntity>().Update(entity);
        await context.SaveChangesAsync(cancellationToken);
        return entry.Entity;
    }

    public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        context.Set<TEntity>().UpdateRange(entities);
        await context.SaveChangesAsync(cancellationToken);
        return entities;
    }
}
