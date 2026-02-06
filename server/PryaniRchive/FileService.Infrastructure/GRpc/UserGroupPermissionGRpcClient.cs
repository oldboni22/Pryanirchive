using Common.Data.Enums;
using Common.ResultPattern;
using FileService.Application.GRpc;
using GRpc.UserPermissions;
using GRpcContracts;

namespace FileService.Infrastructure.GRpc;

public class UserGroupPermissionGRpcClient(UserPermissionService.UserPermissionServiceClient client) : IUserGroupPermissionGRpcClient
{   
    public async Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        var request = new UserSpaceRequest
        {
            UserId = userId.ToString(),
            GroupId = groupId.ToString()
        };

        try 
        {
            var result = await client.GetUserSpacePermissionAsync(request, cancellationToken: cancellationToken);
            
            if (result is null)
            {
                return GRpcErrors.GRpcResponseEmpty;
            }
            
            return Enum.TryParse<UserPermission>(result.Permission, out var permission)
                ? permission
                : new InvalidCastException($"The permission value '{result.Permission}' is not recognized.");
        }
        catch (Exception ex)
        {
            return ex; 
        }
    }
}
