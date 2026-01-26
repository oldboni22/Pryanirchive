namespace Common.Blob;

public class FileOutput
{
    public required string ContentType { get; init; }
    
    public required Stream Content { get; init; }
    
    public required string FileName { get; init; }
}
