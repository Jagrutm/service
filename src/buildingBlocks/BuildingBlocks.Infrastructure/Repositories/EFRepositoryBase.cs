using BuildingBlocks.Application.Contracts.Persistence;
using BuildingBlocks.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BuildingBlocks.Infrastructure.Repositories;

public abstract class EFRepositoryBase<TEntity, TPrimaryKey> : IEFRepository<TEntity, TPrimaryKey>
    where TEntity : BaseAuditEntity<TPrimaryKey>
{
    protected DbContext _dbContext { get; }

    public EFRepositoryBase(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
        string includeString = null, 
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        if (disableTracking) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> predicate = null, 
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, 
        List<Expression<Func<TEntity, object>>> includes = null, 
        bool disableTracking = true)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();
        if (disableTracking) query = query.AsNoTracking();

        if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate != null) query = query.Where(predicate);

        if (orderBy != null)
            return await orderBy(query).ToListAsync();
        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> GetByIdAsync(TPrimaryKey id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        entity.Version = 1;
        entity.CreatedOn = DateTime.UtcNow;
        entity.LastModifiedOn = DateTime.UtcNow;

        _dbContext.Set<TEntity>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        entity.Version = entity.Version + 1;
        entity.LastModifiedOn = DateTime.UtcNow;

        _dbContext.Set<TEntity>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    protected virtual IQueryable<TEntity> Query(bool includeChildEntities = false, bool shouldTrackChanges = false)
    {
        var queryable = shouldTrackChanges
            ? _dbContext.Set<TEntity>()
            : _dbContext.Set<TEntity>().AsNoTrackingWithIdentityResolution();

        return includeChildEntities ? IncludeChildEntitiesIn(queryable) : queryable;
    }

    protected abstract IQueryable<TEntity> IncludeChildEntitiesIn(IQueryable<TEntity> queryable);
}
