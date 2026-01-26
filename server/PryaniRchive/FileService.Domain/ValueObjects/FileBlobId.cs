using Common.ResultPattern;

namespace FileService.Domain.ValueObjects;

public record FileBlobId
{
    private const int GuidLength = 36;
    
    private const int ExtensionMaxLength = 10;
    
    public const int MaxLength = GuidLength + ExtensionMaxLength;
    
    public string Value { get; init; }
    
    private FileBlobId(string value) =>  Value = value;

    public static Result<FileBlobId> Create(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return DomainErrors.EmptyFileName;
        }
        
        var extension = Path.GetExtension(fileName);

        if (extension.Length > ExtensionMaxLength)
        {
            return DomainErrors.FileExtensionTooLarge;
        }

        return new FileBlobId($"{Guid.NewGuid()}{extension.ToLower()}");
    }

    public static FileBlobId FromDatabase(string value) => new FileBlobId(value);
    
    public static implicit operator string(FileBlobId value) => value.Value;
    
    public override string ToString() => Value;
}
