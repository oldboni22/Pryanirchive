using Common.ResultPattern;
using FileService.Application.Contracts.Blob;
using FileService.Infrastructure.Blob.MinIo;
using Grpc.Core;
using GRpc.UserAvatar;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure.GRpc;

public class UserAvatarGRpcService([FromKeyedServices(AvatarMinioService.Key)] IBlobService blobService) 
    : UserAvatarService.UserAvatarServiceBase
{
    public override async Task<UserAvatarLinkResponse> GetUserAvatarLink(UserAvatarLinkRequest request, ServerCallContext context)
    {
        var result = await blobService.GetLoadLinkAsync(request.AvatarId, request.AvatarId, true, context.CancellationToken);

        return new UserAvatarLinkResponse
        {
            Link = result.IsSuccess ? result.Value : string.Empty
        };
    }

    public override async Task<UserAvatarUploadLinkResponse> GetUserAvatarUploadLink(UserAvatarUploadLinkRequest request, ServerCallContext context)
    {
        var result = await blobService.GetUploadLinkAsync(
            request.AvatarId, request.ContentType, AvatarMinioService.MaxAvatarSize, context.CancellationToken);

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
