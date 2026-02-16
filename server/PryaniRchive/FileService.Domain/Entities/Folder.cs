using Common.Data;
using FileService.Domain.ValueObjects;

namespace FileService.Domain.Entities;

public sealed class Folder : EntityBase, IEntityWithTimestamps
{
    public required FolderName Name { get; set; }
    
    public required Guid SpaceId { get; init; }

    public Space Space { get; init; } = null!;
    
    public IEnumerable<Folder> NestedFolders { get; init; } = [];
    
    public Guid? ParentFolderId { get; set; }
    
    public Folder? Parent { get; init; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public IEnumerable<FileReference> Files { get; init; } = [];
    
    public required FolderPath FullPath { get; set; }
}
