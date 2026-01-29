using Common.Data;
using Common.Data.Enums;

namespace UserService.Domain.Entities;

public sealed class UserSpacePermission : EntityWithTimestamps
{
    public Guid SpaceId { get; init; }
    
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;
    
    public required UserPermission Permission { get; set; }
}
