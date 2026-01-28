using Common.Data;

namespace FileService.Domain.Entities;

public class Space : EntityBase
{
    public Guid OwnerId { get; init; }
    
    public required string Name { get; init; }
}
