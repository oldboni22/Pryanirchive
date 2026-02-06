using System;
using System.Threading;
using System.Threading.Tasks;
using Common.ResultPattern;
using Microsoft.Extensions.Caching.Hybrid;

namespace Common.Cache;

public interface ICachedResource<in TKey, TValue>
{
    Task<Result<TValue>> GetAsync(TKey key, CancellationToken cancellationToken = default);

    Task<Result> SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    Task<Result> CreateAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    Task<Result> RemoveAsync(TKey key, CancellationToken cancellationToken = default);
}

public abstract class CachedResource<TKey, TValue>(HybridCache cache) : ICachedResource<TKey, TValue>
{
    protected HybridCache Cache { get; } = cache;
    
    protected abstract HybridCacheEntryOptions Options { get; }
    
    protected abstract string Prefix { get; }
    
    protected abstract string ConvertKey(TKey key); 
    
    protected abstract ValueTask<TValue?> GetFromResourceAsync(TKey key, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask DeleteFromResourceAsync(TKey key, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask SetResourceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    protected abstract ValueTask CreateResourceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    
    public async Task<Result<TValue>> GetAsync(TKey key, CancellationToken cancellationToken = default)
    {
        var result = await Cache.GetOrCreateAsync(
            GenerateKey(key),
            ctx => GetFromResourceAsync(key, ctx),
            Options,
            cancellationToken: cancellationToken);

        if (result is null)
        {
            return CacheErrors.NotFound;
        }
        
        return result;
    }

    public async Task<Result> SetAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        try
        {
            await SetResourceAsync(key, value, cancellationToken);

            return await SaveCacheSet(key, value, cancellationToken);
        }
        catch(Exception ex)
        {
            return ex;
        }
        
    }

    public async Task<Result> CreateAsync(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        try
        {
            await CreateResourceAsync(key, value, cancellationToken);
            return await SaveCacheSet(key, value, cancellationToken);
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
    
    public async Task<Result> RemoveAsync(TKey key, CancellationToken cancellationToken = default)
    {
        try
        {
            await DeleteFromResourceAsync(key, cancellationToken);
            await Cache.RemoveAsync(GenerateKey(key), cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
    
    protected async Task<Result> SaveCacheSet(TKey key, TValue value, CancellationToken cancellationToken = default)
    {
        var generatedKey = GenerateKey(key);
        
        try
        {
            await Cache.SetAsync(generatedKey, value, Options, cancellationToken: cancellationToken);
            return Result.Success();
        }
        catch(Exception ex)
        {
            await Cache.RemoveAsync(generatedKey, cancellationToken);
            return ex;
        }
    }

    private string GenerateKey(TKey key) => $"{Prefix}_{ConvertKey(key)}";
}
