using Common.ResultPattern;
using FileService.Domain.Entities;

namespace FileService.Domain.ValueObjects;

public record FolderPath
{
    public const int MaxLength = 4096;

    private FolderPath(string value) => Value = value;
    
    public string Value { get; }

    public static Result<FolderPath> Create(Guid spaceId, Guid folderId, Folder? parent = null)
    {
        var nestPath = parent is null 
            ? spaceId.ToString("N") 
            : parent.FullPath.ToString();

        var path = $"{nestPath}/{folderId.ToString()}";

        if (path.Length > MaxLength)
        {
            return FileServiceDomainErrors.FolderPathTooLarge;
        }
        
        return new FolderPath(path);
    }
    
    public override string ToString() => Value;
    
    public static FolderPath FromDatabase(string value) => new FolderPath(value);
}
