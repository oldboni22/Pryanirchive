namespace FileService.Infrastructure.Blob.MinIo;

public class MinIoBlobOptions
{
    public const string ConfigSection = "Minio";
    
    public required string Endpoint { get; init; }

    public required string AccessKey { get; init; }
    
    public required string SecretKey { get; init; }
    
    public required string FileBucketName { get; init; }
    
    public required string AvatarBucketName { get; init; }
    
    public required int UrlExpireHours { get; init; } 
}
