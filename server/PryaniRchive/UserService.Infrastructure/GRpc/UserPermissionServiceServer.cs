using Grpc.Core;
using GRpc.UserPermissions;
using UserService.Domain.RepositoryContracts;

namespace UserService.Infrastructure.GRpc;

public class UserPermissionServiceServer(IUserServiceRepositoryManager repositoryManager) : UserPermissionService.UserPermissionServiceBase
{
    private static Status GetInvalidGuidStatus() => new Status(StatusCode.InvalidArgument, "Invalid GUID format"); 
    
    public override async Task<UserGroupReply> GetUserGroupPermission(UserGroupRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId) || !Guid.TryParse(request.GroupId, out var groupId))
        {
            throw new RpcException(GetInvalidGuidStatus());
        }

        var result = await repositoryManager.UserGroupPermissionRepository.GetUserGroupPermissionAsync(userId, groupId);
        
        return new UserGroupReply
        {
            Permission = result.IsSuccess? result.Value.ToString() : string.Empty
        };
    }
}
