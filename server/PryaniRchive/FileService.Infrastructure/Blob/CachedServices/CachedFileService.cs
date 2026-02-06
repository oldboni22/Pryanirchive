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
    : CachedResource<string,string>(cache), ICachedFileService 
{
    protected override HybridCacheEntryOptions Options { get; } =
        new HybridCacheEntryOptions
        {
            Expiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
            LocalCacheExpiration = TimeSpan.FromHours(options.Value.UrlExpireHours),
        };

    protected override string Prefix => "File";

    protected override string ConvertKey(string key) => key;

    protected override async ValueTask DeleteFromResourceAsync(string key, CancellationToken cancellationToken = default)
    {
        await blob.DeleteFileAsync(key, cancellationToken);
    }

    public async Task<Result> SetAsync(string key, Stream value, string contentType, CancellationToken cancellationToken = default)
    {
        var uploadResult = await blob.UploadFileAsync(value, key, contentType, cancellationToken);

        if (!uploadResult.IsSuccess) 
        { 
            return uploadResult;
        }
        
        var linkResult = await blob.GetFileLinkAsync(key, key, true, cancellationToken);

        if (!linkResult.IsSuccess)
        {
            return linkResult;
        }
        
        var cacheResult = await SaveCacheSet(key, linkResult, cancellationToken);
        
        return cacheResult.IsSuccess ? linkResult : cacheResult;
    }

    public async Task<Result<string>> GetAsync(string key, string fileName, bool isInline, CancellationToken cancellationToken = default)
    {
        var mode = isInline ? "inline" : "attachment";
        var modeKey = $"{key}_{mode}";
        
        try
        {
            var result = await Cache.GetOrCreateAsync(
                GenerateKey(modeKey),
                async ctx => await blob.GetFileLinkAsync(key, fileName, isInline, ctx),
                Options,
                tags: [GenerateKey(key)],
                cancellationToken);
            
            return result;
        }
        catch (Exception ex)
        {
            return ex;
        }
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
    
    protected override ValueTask<string?> GetFromResourceAsync(string key, CancellationToken cancellationToken = default)
    {
        return ValueTask.FromResult<string?>(null);
    }
    #endregion
}
