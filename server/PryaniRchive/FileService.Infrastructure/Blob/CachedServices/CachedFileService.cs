using Common.Cache;
using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using FileService.Infrastructure.Blob.MinIo;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Blob.CachedServices;

public class CachedFileService(
    HybridCache cache, 
    [FromKeyedServices(FileMinioService.Key)] IBlobService blob, 
    IOptions<MinIoBlobOptions> options) 
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
        var uploadResult = await blob.UploadFileAsync(value, key, contentType, cancellationToken);

        if (!uploadResult.IsSuccess) 
        { 
            return uploadResult;
        }
        
        await Cache.RemoveByTagAsync(key, cancellationToken);
        
        return uploadResult;
    }

    public async Task<Result> RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var deleteResult = await  blob.DeleteFileAsync(key, cancellationToken);

        if (deleteResult.IsSuccess)
        {
            await Cache.RemoveByTagAsync(key, cancellationToken);
        } 
        
        return deleteResult;
    }

    public async Task<Result<string>> GetAsync(string key, string fileName, bool isInline, CancellationToken cancellationToken = default)
    {
        var modeKey = GenerateModeKey(key, isInline);
        
        try
        {
            var result = await Cache.GetOrCreateAsync(
                modeKey,
                async ctx => await blob.GetFileLinkAsync(key, fileName, isInline, ctx),
                Options,
                tags: [key],
                cancellationToken);
            
            return result;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private static string GenerateModeKey(string key, bool isInline)
    {
        var mode = isInline ? "inline" : "attachment";
        return $"{key}_{mode}";
    }
}
