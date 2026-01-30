using System.Collections.Generic;
using Common.Data;
using UserService.Domain.ValueObjects;

namespace UserService.Domain.Entities;

public sealed class User : EntityWithTimestamps
{
    public required string Name { get; set; }
    
    public required UserTag Tag { get; init; }

    public IEnumerable<UserSpacePermission> Permissions { get; init; } = [];
}
