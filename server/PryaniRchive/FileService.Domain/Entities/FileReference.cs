using Common.Data;
using Common.ResultPattern;
using FileService.Domain.ValueObjects;

namespace FileService.Domain.Entities;

public sealed class FileReference : EntityBase, IEntityWithTimestamps
{
    public Guid FolderId { get; set; }

    public Guid SpaceId { get; init; }

    public Space Space { get; set; } = null!;
    
    public Folder Folder { get; set; } = null!;
    
    public long FileSize { get; set; }
    
    public FileBlobId FileBlobId {get; init;}
    
    public FileName FileName { get; private set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    private FileReference() {}
    
    public static Result<FileReference> Create(Guid generatedFileId, string name, Folder folder, long fileSize)
    {
        var nameResult = FileName.Create(name);

        return nameResult.IsSuccess
            ? new FileReference
            {
                FolderId = folder.Id, 
                FileName = nameResult, 
                FileSize = fileSize, 
                FileBlobId = FileBlobId.Create(generatedFileId, name), 
                SpaceId =  folder.SpaceId
            }
            : nameResult.Error;
    }
}
