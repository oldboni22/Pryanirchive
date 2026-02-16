using Common.Data;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities;

public sealed class User : EntityBase
{
    public required UserTag Tag { get; init; }

    public UserAvatarId? AvatarId { get; set; }

    public IEnumerable<UserSpacePermission> Permissions { get; init; } = [];

    public required UserName Name { get; set; }
}
