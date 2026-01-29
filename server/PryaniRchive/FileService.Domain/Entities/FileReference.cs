using Common.Data;
using Common.ResultPattern;
using FileService.Domain.ValueObjects;

namespace FileService.Domain.Entities;

public sealed class FileReference : EntityWithTimestamps
{
    public Guid FolderId { get; set; }

    public Guid SpaceId { get; init; }

    public Space Space { get; set; } = null!;
    
    public Folder Folder { get; set; } = null!;
    
    public long FileSize { get; set; }
    
    public FileBlobId FileBlobId {get; set;}
    
    public FileName FileName { get; private set; }

    private FileReference() {}

    public Result<FileName> UpdateFileName(string input)
    {
        var nameResult = FileName.Create(input);

        if (nameResult.IsSuccess)
        {
            FileName = nameResult;    
        }
        
        return nameResult;
    }
    
    public static Result<FileReference> Create(string name, Folder folder, long fileSize)
    {
        var nameResult = FileName.Create(name);

        return nameResult.IsSuccess
            ? new FileReference
            {
                FolderId = folder.Id, 
                FileName = nameResult, 
                FileSize = fileSize, 
                FileBlobId = FileBlobId.Create(name), 
                SpaceId =  folder.SpaceId
            }
            : nameResult.Error;
    }
}
