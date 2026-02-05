using Common.Data;

namespace FileService.Domain.Entities;

/// <summary>
/// Represents a folder (group) within a user's hierarchical file system.
/// </summary>
/// <remarks>
/// <b>Indexing Strategy and Uniqueness Rules:</b>
/// <list type="table">
///   <listheader>
///     <term>Index / Purpose</term>
///     <description>Logic and Constraint</description>
///   </listheader>
///   <item>
///     <term><b>Unique Root</b></term>
///     <description>
///     (OwnerId, Name) WHERE ParentGroupId IS NULL. 
///     Ensures folder name uniqueness in the user's root directory.
///     </description>
///   </item>
///   <item>
///     <term><b>Unique Nested</b></term>
///     <description>
///     (OwnerId, Name, ParentGroupId) WHERE ParentGroupId IS NOT NULL. 
///     Ensures folder name uniqueness within any specific subfolder.
///     </description>
///   </item>
///   <item>
///     <term><b>Home Navigation</b></term>
///     <description>
///     (OwnerId) WHERE ParentGroupId IS NULL. 
///     Optimizes the initial load of the user's "Home" or "Root" view.
///     </description>
///   </item>
///   <item>
///     <term><b>Deep Navigation</b></term>
///     <description>
///     (OwnerId, ParentGroupId). 
///     A composite index that allows instantaneous retrieval of subfolders when a user opens a directory.
///     </description>
///   </item>
/// </list>
/// </remarks>
public sealed class Folder : EntityWithTimestamps
{
    /// <summary>
    /// The display name of the folder. Limited to 64 characters.
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// The owner of the folder. All search operations and uniqueness constraints 
    /// are isolated to this specific User ID.
    /// </summary>
    public required Guid SpaceId { get; init; }

    public Space Space { get; set; } = null!;
    
    /// <summary>
    /// Collection of child groups. 
    /// Managed via <see cref="Microsoft.EntityFrameworkCore.DeleteBehavior.Restrict"/> 
    /// to prevent accidental deletion of populated trees.
    /// </summary>
    public IEnumerable<Folder> NestedFolders { get; init; } = [];
    
    /// <summary>
    /// The ID of the parent group. Null if the group is located in the root directory.
    /// </summary>
    public Guid? ParentFolderId { get; set; }
    
    /// <summary>
    /// Navigation property for the parent group.
    /// </summary>
    public Folder? Parent { get; set; }
    
    /// <summary>
    /// Files stored directly within this group/folder.
    /// </summary>
    public IEnumerable<FileReference> Files { get; init; } = [];
}
