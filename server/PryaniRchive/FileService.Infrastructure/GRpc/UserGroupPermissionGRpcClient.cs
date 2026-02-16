using Common.Data.Enums;
using Common.Logging;
using Common.ResultPattern;
using FileService.Application.GRpc;
using GRpc.UserPermissions;
using GRpcContracts;
using Microsoft.Extensions.Logging;

namespace FileService.Infrastructure.GRpc;

public class UserGroupPermissionGRpcClient(
    UserPermissionService.UserPermissionServiceClient client,
    ILogger<UserGroupPermissionGRpcClient> logger) : IUserGroupPermissionGRpcClient
{
    private const string ServiceName = "UserPermissionService";
    
    public async Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId,
        CancellationToken cancellationToken = default)
    {
        const string methodName = "GetUserGroupPermission";
        
        logger.LogGRpcCallStarted(ServiceName, methodName);

        var request = new UserSpaceRequest
        {
            UserId = userId.ToString(),
            SpaceId = groupId.ToString()
        };

        var result = await client.GetUserSpacePermissionAsync(request, cancellationToken: cancellationToken);

        if (result is null)
        {
            logger.LogGRpcCallFailed(new InvalidOperationException("gRPC response was null"), ServiceName, methodName);
            return GRpcErrors.EmptyResponse;
        }

        logger.LogGRpcCallCompleted(ServiceName, methodName);

        return Enum.TryParse<UserPermission>(result.Permission, out var permission)
            ? permission
            : throw new InvalidCastException($"The permission value '{result.Permission}' is not recognized.");
    }
}