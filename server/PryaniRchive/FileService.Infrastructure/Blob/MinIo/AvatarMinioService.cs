using Microsoft.Extensions.Options;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public class AvatarMinioService(IMinioClient client, IOptions<MinIoBlobOptions> options) : MinIoBlobService(client, options)
{
    public const string Key = "Avatar";
    
    protected override string BucketName { get; } = options.Value.AvatarBucketName;
}
