using Common.Cache;
using Common.ResultPattern;
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
    : CachedResource(cache), ICachedAvatarService
{
    private const int ExpirationOffset = 30;
    
    protected override HybridCacheEntryOptions Options { get; } =
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromHours(options.Value.UrlExpireHours).Subtract(TimeSpan.FromMinutes(ExpirationOffset)),
            LocalCacheExpiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
        };

    protected override string? Prefix => null; //Not needed
    
    public async Task<Result<string>> GetAsync(Guid userId, string key, CancellationToken cancellationToken = default)
    {
        var link = await Cache.GetOrCreateAsync(
            key,
            async ctx =>
            {
                var result = await blob.GetFileLinkAsync(key, key, false, ctx);
                
                return result.IsSuccess ? result.Value : null;
            },
            Options,
            [userId.ToString()],
            cancellationToken: cancellationToken);

        if (link is null)
        {
            return Error.NotFound;
        }
        
        return link;
    }

    public async Task<Result<string>> SetAsync(Guid userId, string key, Stream value, string contentType, CancellationToken cancellationToken = default)
    {
        var uploadResult = await blob.UploadFileAsync(value, key, contentType, cancellationToken);

        if (!uploadResult.IsSuccess)
        {
            return uploadResult.Error;
        }
        
        var linkResult = await blob.GetFileLinkAsync(key, key, true, cancellationToken);

        if (!linkResult.IsSuccess)
        {
            return linkResult;
        }

        try
        {
            await Cache.RemoveByTagAsync(userId.ToString(), cancellationToken);
        }
        catch
        {
            //log
        }
        
        return linkResult;
    }

    public async Task<Result> RemoveAsync(Guid userId, string key, CancellationToken cancellationToken = default)
    {
        var removeResult = await blob.DeleteFileAsync(key, cancellationToken);

        if (removeResult.IsSuccess)
        {
            await Cache.RemoveByTagAsync(userId.ToString(), cancellationToken);
        }
        
        return removeResult;
    }
}
