using Grpc.Core;
using GRpc.UserPermissions;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.GRpc;

public class UserPermissionServiceServer(UserServiceDbContext dbContext) : UserPermissionService.UserPermissionServiceBase
{
    private static Status GetInvalidGuidStatus() => new Status(StatusCode.InvalidArgument, "Invalid GUID format"); 
    
    private static Status GetPermissionNotExistStatus() => new Status(StatusCode.NotFound, "User permission not found"); 
    
    public override async Task<UserSpaceReply> GetUserSpacePermission(UserSpaceRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.UserId, out var userId) || !Guid.TryParse(request.SpaceId, out var groupId))
        {
            throw new RpcException(GetInvalidGuidStatus());
        }

        var permission = await dbContext.UserPermissions
                             .AsNoTracking()
            .Where(p => p.UserId == userId && p.SpaceId == groupId)
                             .Select(p => new { p.Permission })
            .FirstOrDefaultAsync()
            ?? throw new RpcException(GetPermissionNotExistStatus());

        return new UserSpaceReply { Permission = permission.ToString() };
    }
}
