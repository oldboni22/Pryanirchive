using Common.Data.Enums;
using Common.ResultPattern;
using FileService.Application.GRpc;
using GRpc.UserPermissions;

namespace FileService.Infrastructure.GRpc;

public class UserGroupPermissionGRpcClient(UserPermissionService.UserPermissionServiceClient client) : IUserGroupPermissionGRpcClient
{   
    public async Task<Result<UserPermission>> GetUserGroupPermissionAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default)
    {
        var request = new UserGroupRequest
        {
            UserId = userId.ToString(),
            GroupId = groupId.ToString()
        };

        try 
        {
            var result = await client.GetUserGroupPermissionAsync(request, cancellationToken: cancellationToken);
            
            if (result is null || string.IsNullOrEmpty(result.Permission))
            {
                return FileServiceInfraErrors.NotAllowedAction;
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
