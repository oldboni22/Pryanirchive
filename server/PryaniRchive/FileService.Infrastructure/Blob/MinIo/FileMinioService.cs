using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;

namespace FileService.Infrastructure.Blob.MinIo;

public class FileMinioService(IMinioClient client, IOptions<MinIoBlobOptions> options, ILogger<MinIoBlobService> logger) 
    : MinIoBlobService(client, options, logger)
{
    public const string Key = "File";
    
    protected override string BucketName { get; } =  options.Value.FileBucketName;
}
