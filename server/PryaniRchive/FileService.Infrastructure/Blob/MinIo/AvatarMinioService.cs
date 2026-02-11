using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public class AvatarMinioService(IMinioClient client, IOptions<MinIoBlobOptions> options, ILogger<MinIoBlobService> logger) 
    : MinIoBlobService(client, options, logger)
{
    public const string Key = "Avatar";
    
    public const int MaxAvatarSize = 5 * 1024 * 1024;
    
    protected override string BucketName { get; } = options.Value.AvatarBucketName;
}
