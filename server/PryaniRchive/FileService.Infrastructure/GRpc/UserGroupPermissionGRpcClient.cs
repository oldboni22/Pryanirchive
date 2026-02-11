using Common.Data.Enums;
using Common.Logging;
using Common.ResultPattern;
using FileService.Application.GRpc;
using GRpc.UserPermissions;
using GRpcContracts;
using Microsoft.Extensions.Logging;

namespace FileService.Infrastructure.GRpc;

public class UserGroupPermissionGRpcClient(UserPermissionService.UserPermissionServiceClient client, ILogger<UserGroupPermissionGRpcClient> logger) : IUserGroupPermissionGRpcClient
{   
    public async Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        logger.LogGRpcCallStarted("UserPermissionService", "GetUserSpacePermission");
        
        var request = new UserSpaceRequest
        {
            UserId = userId.ToString(),
            SpaceId = groupId.ToString()
        };

        try 
        {
            var result = await client.GetUserSpacePermissionAsync(request, cancellationToken: cancellationToken);
            
            if (result is null)
            {
                logger.LogGRpcCallFailed(new InvalidOperationException("gRPC response was null"), "UserPermissionService", "GetUserSpacePermission");
                return GRpcErrors.GRpcResponseEmpty;
            }
            
            logger.LogGRpcCallCompleted("UserPermissionService", "GetUserSpacePermission");
            
            return Enum.TryParse<UserPermission>(result.Permission, out var permission)
                ? permission
                : new InvalidCastException($"The permission value '{result.Permission}' is not recognized.");
        }
        catch (Exception ex)
        {
            logger.LogGRpcCallFailed(ex, "UserPermissionService", "GetUserSpacePermission");
            return ex; 
        }
    }
}
