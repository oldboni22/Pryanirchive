using Common.Data;
using Common.Data.Enums;
using Common.ResultPattern;
using UserService.Domain.Entities;

namespace UserService.Domain.RepositoryContracts;

public interface IUserSpacePermissionRepository : IRepository<UserSpacePermission>
{
    Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId);
}
