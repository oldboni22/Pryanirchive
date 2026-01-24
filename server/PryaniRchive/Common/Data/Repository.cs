using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Common.Data;

public interface IRepository<T> where T : EntityBase
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    
    Task<T?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default);
    
    Task<T?> UpdateAsync(Guid id, T updatedEntity, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public class Repository<T>(DbContext context) : IRepository<T> where T : EntityBase
{
    protected DbContext Context { get; } = context;
    
    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        var createdEntity = await Context.Set<T>().AddAsync(entity, cancellationToken);

        return createdEntity.Entity;
    }

    public Task<T?> GetByIdAsync(Guid id, bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return trackChanges
            ? Context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken)
            : Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<T?> UpdateAsync(Guid id, T updatedEntity, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, true, cancellationToken);

        if (entity is null)
        {
            return null;
        }
        
        updatedEntity.Id = id;
        Context.Entry(entity).CurrentValues.SetValues(updatedEntity);

        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, false, cancellationToken);

        if (entity is null)
        {
            return false;
        }
        
        Context.Set<T>().Remove(entity);
        return true;
    }
    
    protected async Task<PagedList<T>> GetByConditionAsync(
        Expression<Func<T, bool>> expression, 
        PaginationParameters paginationParameters,
        bool trackChanges = false,
        CancellationToken cancellationToken = default)
    {
        var query = Context
            .Set<T>()
            .Where(expression);
        
        var count = await query.CountAsync(cancellationToken);
        
        query = query
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize);

        query = trackChanges 
            ? query
            : query.AsNoTracking();
        
        var list = await query.ToListAsync(cancellationToken);
        
        return list.ToPagedList(paginationParameters.PageNumber, paginationParameters.PageSize, count);
    }
} 
