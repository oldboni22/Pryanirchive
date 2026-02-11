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

public class CachedFileService(
    HybridCache cache, 
    [FromKeyedServices(FileMinioService.Key)] IBlobService blob, 
    IOptions<MinIoBlobOptions> options,
    ILogger<CachedFileService> logger) 
    : CachedResource(cache), ICachedFileService 
{
    private const int ExpirationOffset = 30;
    
    protected override HybridCacheEntryOptions Options { get; } =
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromHours(options.Value.UrlExpireHours).Subtract(TimeSpan.FromMinutes(ExpirationOffset)),
            LocalCacheExpiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
        };

    protected override string? Prefix => null; //Not needed

    public async Task<Result> SetAsync(string key, Stream value, string contentType, CancellationToken cancellationToken = default)
    {
        logger.LogCacheSetStarted(key);
        
        var uploadResult = await blob.UploadFileAsync(value, key, contentType, cancellationToken);

        if (!uploadResult.IsSuccess) 
        { 
            return uploadResult;
        }
        
        try
        {
            await Cache.RemoveByTagAsync(key, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogCacheOperationFailed(ex, key);
        }
        
        return uploadResult;
    }

    public async Task<Result> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        logger.LogCacheRemoveStarted(key);
        
        var deleteResult = await  blob.DeleteFileAsync(key, cancellationToken);

        if (deleteResult.IsSuccess)
        {
            try
            {
                await Cache.RemoveByTagAsync(key, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogCacheOperationFailed(ex, key);
            }
        } 
        
        return deleteResult;
    }

    public async Task<Result<string>> GetAsync(string key, string fileName, bool isInline, CancellationToken cancellationToken = default)
    {
        var modeKey = GenerateModeKey(key, isInline);
        
        try
        {
            var cacheHit = false;
            
            var result = await Cache.GetOrCreateAsync(
                modeKey,
                async ctx =>
                {
                    logger.LogCacheMiss(modeKey);
                    return await blob.GetFileLinkAsync(key, fileName, isInline, ctx);
                },
                Options,
                tags: [key],
                cancellationToken);
            
            if (result.IsSuccess && !cacheHit)
            {
                logger.LogCacheHit(modeKey);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogCacheOperationFailed(ex, modeKey);
            return ex;
        }
    }

    private static string GenerateModeKey(string key, bool isInline)
    {
        var mode = isInline ? "inline" : "attachment";
        return $"{key}_{mode}";
    }
}
