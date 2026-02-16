using Common.ResultPattern;
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

        var result = await client.GetUserAvatarLinkAsync(request, cancellationToken: cancellationToken);

        if (result is null)
        {
            return GRpcErrors.EmptyResponse;
        }

        return result.Link;
    }

    public async Task<Result<string>> GetUploadLinkAsync(
        UserAvatarId avatarId, string contentType, CancellationToken cancellationToken = default)
    {
        var request = new UserAvatarUploadLinkRequest
        {
            AvatarId = avatarId,
            ContentType = contentType
        };

        var result = await client.GetUserAvatarUploadLinkAsync(request, cancellationToken: cancellationToken);

        if (result is null)
        {
            return GRpcErrors.EmptyResponse;
        }

        return result.UploadLink;
    }
}