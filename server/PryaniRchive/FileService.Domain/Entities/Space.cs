using Common.Data;
using FileService.Domain.ValueObjects;

namespace FileService.Domain.Entities;

public class Space : EntityBase
{
    public Guid OwnerId { get; init; }
    
    public required SpaceName Name { get; set; }
}
