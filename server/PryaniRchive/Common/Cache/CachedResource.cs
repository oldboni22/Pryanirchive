using Microsoft.Extensions.Caching.Hybrid;

namespace Common.Cache;

public abstract class CachedResource(HybridCache cache)
{
    protected HybridCache Cache { get; } = cache;
    
    protected abstract HybridCacheEntryOptions Options { get; }
    
    protected abstract string? Prefix { get; }
    
    protected virtual string GenerateKey(string key) => Prefix is null ? key : $"{Prefix}_{key}";
}