using System.Threading;
using System.Threading.Tasks;
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
    
    protected abstract HybridCacheEntryOptions Options { get; }
    
    protected abstract string Prefix { get; }
    
    protected abstract string ConvertKey(TKey key); 
    
    protected abstract ValueTask<TValue> GetFromResourceAsync(TKey key, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask DeleteFromResourceAsync(TKey key, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask SetResourceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask CreateResourceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    public async Task<TValue> GetAsync(TKey key, CancellationToken cancellationToken = default)
    {
        return await Cache.GetOrCreateAsync(
            GenerateKey(key),
            ctx => GetFromResourceAsync(key, ctx),
            Options,
            cancellationToken: cancellationToken);
    }

    public async Task SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await SetResourceAsync(key, value, cancellationToken);
        await SaveCacheSet(GenerateKey(key), value, cancellationToken);
    }

    public async Task CreateAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        await CreateResourceAsync(key, value, cancellationToken);
        await SaveCacheSet(GenerateKey(key), value, cancellationToken);
    }
    
    public async Task RemoveAsync(TKey key, CancellationToken cancellationToken = default)
    {
        await DeleteFromResourceAsync(key, cancellationToken);
        await Cache.RemoveAsync(GenerateKey(key), cancellationToken);
    }
    
    private async Task SaveCacheSet(string key, TValue value, CancellationToken cancellationToken = default)
    {
        try
        {
            await Cache.SetAsync(key, value, Options, cancellationToken: cancellationToken);
        }
        catch
        {
            await Cache.RemoveAsync(key, cancellationToken);
        }
    }

    private string GenerateKey(TKey key) => $"{Prefix}_{ConvertKey(key)}";
}
