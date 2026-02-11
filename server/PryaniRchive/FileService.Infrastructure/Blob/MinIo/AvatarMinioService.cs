using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public class AvatarMinioService(
    IMinioClient client, 
    IOptions<MinIoConnectionOptions> options, 
    IOptionsSnapshot<MinIoServiceOptions>  serviceOptions,
    ILogger<MinIoBlobService> logger) 
    : MinIoBlobService(client, options, logger)
{
    public const string Key = "Avatar";
    
    public const int MaxAvatarSize = 5 * 1024 * 1024;

    protected override string BucketName { get; } = serviceOptions.Get(MinIoServiceOptions.AvatarKey).BucketName;
    
    protected override int ExpirationSeconds { get; } = serviceOptions.Get(MinIoServiceOptions.AvatarKey).UrlExpireSeconds;
}
