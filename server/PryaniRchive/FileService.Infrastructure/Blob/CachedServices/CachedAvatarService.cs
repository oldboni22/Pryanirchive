using Common.Cache;
using Common.Logging;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using FileService.Infrastructure.Blob.MinIo;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Blob.CachedServices;

public sealed class CachedAvatarService(
    HybridCache cache, 
    [FromKeyedServices(AvatarMinioService.Key)] IBlobService blob, 
    IOptions<MinIoBlobOptions> options,
    ILogger<CachedAvatarService> logger) 
    : CachedResource(cache), ICachedAvatarService
{
    private const int ExpirationOffset = 30;

    private const int MaxAvatarSize = (int)(3.5 * 1024 * 1024);
    
    protected override HybridCacheEntryOptions Options { get; } =
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromHours(options.Value.UrlExpireHours).Subtract(TimeSpan.FromMinutes(ExpirationOffset)),
            LocalCacheExpiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
        };

    protected override string? Prefix => null; //Not needed
    
    public async Task<Result<string>> GetLoadLinkAsync(Guid userId, string key, CancellationToken cancellationToken = default)
    {
        var link = await Cache.GetOrCreateAsync(
            key,
            async ctx =>
            {
                logger.LogCacheMiss(key);
                var result = await blob.GetLoadLinkAsync(key, key, false, ctx);
                
                return result.IsSuccess ? result.Value : null;
            },
            Options,
            [userId.ToString()],
            cancellationToken: cancellationToken);

        if (link is not null)
        {
            logger.LogCacheHit(key);
        }
        
        if (link is null)
        {
            return Error.NotFound;
        }
        
        return link;
    }

    public async Task<Result<FileUploadDto>> GetUploadLinkAsync(Guid userId, string key, string contentType, CancellationToken cancellationToken = default)
    {
        logger.LogBlobOperationStarted("GetUploadLink", key, "Cache");
        
        var linkResult = await blob.GetUploadLinkAsync(key, contentType, MaxAvatarSize, cancellationToken);
        
        if (!linkResult.IsSuccess)
        {
            return linkResult;
        }

        try
        {
            await Cache.RemoveByTagAsync(userId.ToString(), cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogCacheOperationFailed(ex, key);
        }
        
        return linkResult;
    }

    public async Task<Result> RemoveAsync(Guid userId, string key, CancellationToken cancellationToken = default)
    {
        logger.LogCacheRemoveStarted(key);
        
        var removeResult = await blob.DeleteFileAsync(key, cancellationToken);

        if (removeResult.IsSuccess)
        {
            try
            {
                await Cache.RemoveByTagAsync(userId.ToString(), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogCacheOperationFailed(ex, key);
            }
        }
        
        return removeResult;
    }
}
