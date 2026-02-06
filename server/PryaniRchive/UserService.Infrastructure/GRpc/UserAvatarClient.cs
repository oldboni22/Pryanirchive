using System.Net.NetworkInformation;
using Common.ResultPattern;
using Google.Protobuf;
using GRpc.UserAvatar;
using GRpcContracts;
using UserService.Application.GRpc;
using UserService.Domain.ValueObjects;

namespace UserService.Infrastructure.GRpc;

public class UserAvatarClient(UserAvatarService.UserAvatarServiceClient client) : IUserAvatarClient
{
    public async Task<Result<string>> GetUserAvatarLinkAsync(UserAvatarId avatarId, CancellationToken cancellationToken = default)
    {
        var request = new UserAvatarLinkRequest
        {
            AvatarId = avatarId.Value,
        };

        try
        {
            var result = await client.GetUserAvatarLinkAsync(request, cancellationToken: cancellationToken);

            if (result is null)
            {
                return GRpcErrors.GRpcResponseEmpty;
            }

            return result.Link;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    public async Task<Result> UploadUserAvatarAsync(UserAvatarId avatarId, Stream avatarData, CancellationToken cancellationToken = default)
    {
        var request = new UserAvatarUploadRequest
        {
            AvatarId = avatarId,
            Content = await ByteString.FromStreamAsync(avatarData, cancellationToken)
        };

        try
        {
            var result = await client.UploadUserAvatarAsync(request, cancellationToken: cancellationToken);
            
            if (result is null)
            {
                return GRpcErrors.GRpcResponseEmpty;
            }
            
            return result.IsSuccess
                ? Result.Success()
                : new NetworkInformationException();
        }
        catch(Exception ex)
        {
            return ex;
        }
    }
}