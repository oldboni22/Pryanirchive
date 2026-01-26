using Common.Data;
using Common.ResultPattern;
using FileService.Domain.ValueObjects;

namespace FileService.Domain.Entities;

public sealed class GroupFile : EntityWithTimestamps
{
    public Guid GroupId { get; set; }

    public Guid OwnerId { get; init; }
    
    public FileGroup FileGroup { get; set; } = null!;
    
    public long FileSize { get; set; }
    
    public FileBlobId FileBlobId {get; set;}
    
    public FileName FileName { get; private set; }

    private GroupFile() {}

    public Result<FileName> UpdateFileName(string input)
    {
        var nameResult = FileName.Create(input);

        if (nameResult.IsSuccess)
        {
            FileName = nameResult;    
        }
        
        return nameResult;
    }
    
    public static Result<GroupFile> Create(string name, FileGroup group, long fileSize)
    {
        var nameResult = FileName.Create(name);

        return nameResult.IsSuccess
            ? new GroupFile
            {
                GroupId = group.Id, 
                FileName = nameResult, 
                FileSize = fileSize, 
                FileBlobId = FileBlobId.Create(name), 
                OwnerId =  group.OwnerId
            }
            : nameResult.Error;
    }
}
