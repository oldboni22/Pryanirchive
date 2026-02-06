using Common.Data;
using Common.ResultPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;

namespace Common.Cache;

public interface ICachedEntity<TValue> where TValue : EntityBase
{
    Task<Result<TValue>> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Result<TValue>> SetAsync(Guid id, TValue value, CancellationToken cancellationToken = default);
    
    Task<Result<TValue>> CreateAsync(Guid id, TValue value, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default);
}

public abstract class CachedEntity<TValue>(HybridCache cache) : CachedResource(cache), ICachedEntity<TValue> where TValue : EntityBase
{
    protected abstract ValueTask<TValue?> GetFromResourceAsync(Guid id, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask RemoveFromResourceAsync(Guid id, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask<TValue?> SetResourceAsync(Guid id, TValue value, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask<TValue> CreateResourceAsync(Guid id, TValue value, CancellationToken cancellationToken = default);
    
    public async Task<Result<TValue>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(id.ToString()); 
        
        try
        {
            var entity = await Cache.GetOrCreateAsync(
                key,
                ctx => GetFromResourceAsync(id, ctx),
                Options,
                [key],
                cancellationToken);

            return entity is not null ? entity : Error.NotFound;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result<TValue>> SetAsync(Guid id, TValue value, CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(id.ToString());
        
        try
        {
            var updated = await SetResourceAsync(id, value, cancellationToken);

            if (updated is null)
            {
                return Error.NotFound;
            }
            
            try
            {
                await Cache.RemoveByTagAsync(key, cancellationToken);
            }
            catch (Exception ex)
            {
                    
            }
            
            return updated;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result<TValue>> CreateAsync(Guid id, TValue value, CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(id.ToString());
        
        try
        {
            var created = await CreateResourceAsync(id, value, cancellationToken);
            await Cache.RemoveByTagAsync(key, cancellationToken);
            
            return created;
        }
        catch (DbUpdateException)
        {
            return Error.Collision;
        }
        catch (Exception ex)
        {
            return ex;    
        }
    }

    public async Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(id.ToString());
        
        try
        {
            await RemoveFromResourceAsync(id, cancellationToken);
            await Cache.RemoveByTagAsync(key, cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}
