using Common.Cache;
using FileService.Application.Contracts.Blob;
using FileService.Infrastructure.Blob.MinIo;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Blob.CachedServices;

public sealed class CachedAvatarService(
    HybridCache cache, 
    [FromKeyedServices(AvatarMinioService.Key)] IBlobService blob, 
    IOptions<MinIoBlobOptions> options) 
    : CachedResource<string,string>(cache), ICachedAvatarService
{
    protected override HybridCacheEntryOptions Options { get; } =
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
            LocalCacheExpiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
        };

    protected override string Prefix => "UserAvatar";

    protected override string ConvertKey(string key) => key;

    protected override async ValueTask<string> GetFromResourceAsync(string key, CancellationToken cancellationToken = default)
    {
        return await blob.GetFileLinkAsync(key, key, true, cancellationToken);
    }

    protected override async ValueTask DeleteFromResourceAsync(string key, CancellationToken cancellationToken = default)
    {
        await blob.DeleteFileAsync(key, cancellationToken);
    }

    public async Task SetAsync(string key, Stream value, string contentType, CancellationToken cancellationToken = default)
    {
        await blob.UploadFileAsync(value, key, contentType, cancellationToken);
        
        var link = await blob.GetFileLinkAsync(key, key, true, cancellationToken);
        
        await SaveCacheSet(key, link, cancellationToken);
    }

    #region Imposible to implement
    
    protected override ValueTask SetResourceAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }
    
    protected override ValueTask CreateResourceAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        return ValueTask.CompletedTask;
    }
    #endregion
}