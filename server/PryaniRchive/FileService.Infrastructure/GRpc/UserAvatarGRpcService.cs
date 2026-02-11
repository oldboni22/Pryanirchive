using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using Grpc.Core;
using GRpc.UserAvatar;

namespace FileService.Infrastructure.GRpc;

public class UserAvatarGRpcService(ICachedAvatarService cachedAvatarService) 
    : UserAvatarService.UserAvatarServiceBase
{
    public override async Task<UserAvatarLinkResponse> GetUserAvatarLink(UserAvatarLinkRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.AvatarId, out var userId))
        {
             return new UserAvatarLinkResponse { Link = string.Empty };
        }

        var result = await cachedAvatarService.GetLoadLinkAsync(userId, request.AvatarId, context.CancellationToken);

        return new UserAvatarLinkResponse
        {
            Link = result.IsSuccess ? result.Value : string.Empty
        };
    }

    public override async Task<UserAvatarUploadLinkResponse> GetUserAvatarUploadLink(UserAvatarUploadLinkRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.AvatarId, out var userId))
        {
             return new UserAvatarUploadLinkResponse { UploadLink = string.Empty };
        }
        
        var result = await cachedAvatarService.GetUploadLinkAsync(userId, request.AvatarId, request.ContentType, context.CancellationToken);

        if (!result.IsSuccess)
        {
            var statusCode = result.Error == Error.NotFound ? StatusCode.NotFound : StatusCode.Internal;
            throw new RpcException(new Status(statusCode, result.Error.ToString()));
        }

        var response = new UserAvatarUploadLinkResponse
        {
            UploadLink = result.Value.Url,
        };
        
        response.FormData.Add(result.Value.FormData);
            
        return response;
    }
}
