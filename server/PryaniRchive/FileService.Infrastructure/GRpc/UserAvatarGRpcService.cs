using FileService.Application.Contracts.Blob;
using FileService.Infrastructure.Blob.MinIo;
using Grpc.Core;
using GRpc.UserAvatar;
using Microsoft.Extensions.DependencyInjection;

namespace FileService.Infrastructure.GRpc;

public class UserAvatarGRpcService([FromKeyedServices(AvatarMinioService.Key)] IBlobService blobService) 
    : UserAvatarService.UserAvatarServiceBase
{
    const int MaxFileSize = (int)(3.5 * 1024 * 1024); // 3.5MB
    
    public override async Task<UserAvatarLinkResponse> GetUserAvatarLink(UserAvatarLinkRequest request, ServerCallContext context)
    {
        var id = request.AvatarId;

        var link = await blobService.GetFileLinkAsync(id, id, true);

        return new UserAvatarLinkResponse
        {
            Link = link
        };
    }

    public override async Task<UserAvatarUploadResponse> UploadUserAvatar(UserAvatarUploadRequest request, ServerCallContext context)
    {
        if (request.Content.Length > MaxFileSize)
        {
            return new UserAvatarUploadResponse{ IsSuccess = false};
        }
        
        try
        {
            using var stream = new MemoryStream(request.Content.ToByteArray());
            var result = await blobService.UploadFileAsync(stream, request.AvatarId, request.ContentType);
            
            return new UserAvatarUploadResponse
            {
                IsSuccess = result.IsSuccess,
            };
        }
        catch
        {
            return new UserAvatarUploadResponse
            {
                IsSuccess = false,
            };
        }
    }
}
