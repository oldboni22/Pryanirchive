namespace FileService.Infrastructure.Blob.MinIo;

public class MinIoConnectionOptions
{
    public const string ConfigSection = "Minio:Connection";
    
    public required string Endpoint { get; init; }

    public required string AccessKey { get; init; }
    
    public required string SecretKey { get; init; }
}
