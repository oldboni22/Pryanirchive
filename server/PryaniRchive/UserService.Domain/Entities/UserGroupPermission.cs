using Common.Data;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public sealed class UserGroupPermission : EntityWithTimestamps
{
    public Guid FileGroupId { get; init; }
    
    public Guid UserId { get; init; }

    public User User { get; init; } = null!;
    
    public required UserPermission Permission { get; set; }
}
