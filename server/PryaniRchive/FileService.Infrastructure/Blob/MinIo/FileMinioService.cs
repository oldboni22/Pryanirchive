using Microsoft.Extensions.Options;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public class FileMinioService(IMinioClient client, IOptions<MinIoBlobOptions> options) : MinIoBlobService(client, options)
{
    public const string Key = "File";
    
    protected override string BucketName { get; } =  options.Value.FileBucketName;
}
