using Microsoft.Extensions.Caching.Hybrid;

namespace Common.Cache;

public interface ICachedResource<in TKey, TValue>
{
    Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken = default);

    Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    Task CreateAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    Task RemoveAsync(TKey key, CancellationToken cancellationToken = default);
}

public abstract class CachedResource<TKey, TValue>(HybridCache cache) : ICachedResource<TKey, TValue>
{
    protected HybridCache Cache { get; } = cache;
    
    protected abstract string ConvertKey(TKey key); 
    
    protected abstract ValueTask<TValue> GetFromResourceAsync(TKey key, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask DeleteFromResourceAsync(TKey key, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask SetResourceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask CreateResourceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    public async Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await Cache.GetOrCreateAsync(
            ConvertKey(key),
            ctx => GetFromResourceAsync(key, ctx),
            cancellationToken: cancellationToken);
    }

    public async Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await SetResourceAsync(key, value, cancellationToken);
        await Cache.SetAsync(ConvertKey(key), value, cancellationToken: cancellationToken);
    }

    public async Task CreateAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await CreateResourceAsync(key, value, cancellationToken);
        await Cache.SetAsync(ConvertKey(key), value, cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(TKey key, CancellationToken cancellationToken = default)
    {
        await DeleteFromResourceAsync(key, cancellationToken);
        await Cache.RemoveAsync(ConvertKey(key), cancellationToken);
    }
}
