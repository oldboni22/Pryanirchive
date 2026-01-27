using Common.Data;
using Common.ResultPattern;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Domain.RepositoryContracts;

public interface IUserGroupPermissionRepository : IRepository<UserGroupPermission>
{
    Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId);
}
