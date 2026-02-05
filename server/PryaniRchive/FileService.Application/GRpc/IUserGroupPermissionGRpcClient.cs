using Common.Data.Enums;
using Common.ResultPattern;

namespace FileService.Application.GRpc;

public interface IUserGroupPermissionGRpcClient
{
    Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId, CancellationToken cancellationToken =  default);
}
