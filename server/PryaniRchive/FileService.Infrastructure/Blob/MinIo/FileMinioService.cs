using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public class FileMinioService(
    IMinioClient client, 
    IOptions<MinIoConnectionOptions> options, 
    IOptionsSnapshot<MinIoServiceOptions>  serviceOptions,
    ILogger<MinIoBlobService> logger) 
    : MinIoBlobService(client, options, logger)
{
    public const string Key = "File";
    
    protected override string BucketName { get; } = serviceOptions.Get(MinIoServiceOptions.FileKey).BucketName;
    protected override int ExpirationSeconds { get; } = serviceOptions.Get(MinIoServiceOptions.FileKey).UrlExpireSeconds;
}
