using Common.Data;
using Common.ResultPattern;
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
    
    protected abstract ValueTask<Result> RemoveFromResourceAsync(Guid id, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask<Result<TValue?>> SetResourceAsync(Guid id, TValue value, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask<Result<TValue>> CreateResourceAsync(Guid id, TValue value, CancellationToken cancellationToken = default);
    
    public async Task<Result<TValue>> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        
        var key = GenerateKey(id.ToString()); 
            var entity = await Cache.GetOrCreateAsync(
                key,
                ctx => GetFromResourceAsync(id, ctx),
                Options,
                [key],
                cancellationToken);

        return entity is null
            ? Error.NotFound
            : entity;
    }

    public async Task<Result<TValue>> SetAsync(Guid id, TValue value, CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(id.ToString());

        var result = await SetResourceAsync(id, value, cancellationToken);

        if (result.IsSuccess)
        {
            await Cache.RemoveByTagAsync(key, cancellationToken);    
        }

        return result.IsSuccess ? result.Value : result!;
    }

    public async Task<Result<TValue>> CreateAsync(Guid id, TValue value, CancellationToken cancellationToken = default)
    {
        var key = GenerateKey(id.ToString());
        
        var result = await CreateResourceAsync(id, value, cancellationToken);

        if (result.IsSuccess)
        {
            await Cache.RemoveByTagAsync(key, cancellationToken);
        }

        return result;
    }

    public async Task<Result> RemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await RemoveFromResourceAsync(id, cancellationToken);
        
        if(result.IsSuccess)
        {
            var key = GenerateKey(id.ToString());
            await Cache.RemoveByTagAsync(key, cancellationToken);
        }

        return result;
    }
}
