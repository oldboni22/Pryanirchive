using System;
using Common.ResultPattern;

namespace AuthService.Application.Contracts;

public interface IJwtService
{
    TokenPair IssueTokens(Guid userId);

    Result<Guid> ExtractUserIdFromExpiredToken(string expiredToken);
}
