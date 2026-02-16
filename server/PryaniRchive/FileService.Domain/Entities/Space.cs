using Common.Data;
using FileService.Domain.ValueObjects;

namespace FileService.Domain.Entities;

public class Space : EntityBase
{
    public const int MaxNameLength = 25;
    
    public Guid OwnerId { get; init; }
    
    public required SpaceName Name { get; set; }
}
