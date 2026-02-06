using Common.ResultPattern;
using UserService.Domain.ValueObjects;

namespace UserService.Application.GRpc;

public interface IUserAvatarClient
{
    Task<Result<string>> GetUserAvatarLinkAsync(UserAvatarId avatarId, CancellationToken cancellationToken = default);
    
    Task<Result> UploadUserAvatarAsync(UserAvatarId avatarId, Stream avatarData, CancellationToken cancellationToken = default);
}
