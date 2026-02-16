using Common.ResultPattern;

namespace FileService.Domain.ValueObjects;

public record FileBlobId
{
    private const int GuidLength = 36;
    
    private const int ExtensionMaxLength = 10;
    
    public const int MaxLength = GuidLength + ExtensionMaxLength + 7;
    
    public string Value { get; init; }
    
    private FileBlobId(string value) =>  Value = value;

    public static Result<FileBlobId> Create(Guid generatedId, string fileName)
    {
        if (generatedId == Guid.Empty)
        {
            return FileServiceDomainErrors.EmptyBlobId;
        }
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return FileServiceDomainErrors.EmptyFileName;
        }
        
        var extension = Path.GetExtension(fileName);

        if (extension.Length > ExtensionMaxLength)
        {
            return FileServiceDomainErrors.FileExtensionTooLarge;
        }

        return new FileBlobId($"{generatedId}{extension.ToLower()}");
    }

    public static FileBlobId FromDatabase(string value) => new FileBlobId(value);
    
    public static implicit operator string(FileBlobId value) => value.Value;
    
    public override string ToString() => Value;
}
