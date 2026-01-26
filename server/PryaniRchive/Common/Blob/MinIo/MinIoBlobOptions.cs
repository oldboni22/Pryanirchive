namespace Common.Blob.MinIo;

public class MinIoBlobOptions
{
    public const string ConfigSection = "Minio";
    
    public required string BucketName { get; init; }
    
    public required int UrlExpireHours { get; init; } 
}