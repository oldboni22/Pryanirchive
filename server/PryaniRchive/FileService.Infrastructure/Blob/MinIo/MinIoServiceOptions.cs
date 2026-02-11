namespace FileService.Infrastructure.Blob.MinIo;

public class MinIoServiceOptions
{
    private const string PrimaryConfigSection = "Minio";

    public const string FileSection = $"{PrimaryConfigSection}:File";
    
    public const string AvatarSection = $"{PrimaryConfigSection}:Avatar";

    public const string AvatarKey = "Avatar";
    
    public const string FileKey = "Avatar";
    
    public required string BucketName { get; init; }
    
    public int UrlExpireSeconds { get; init; }
}
