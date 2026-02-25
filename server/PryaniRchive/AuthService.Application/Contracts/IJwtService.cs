namespace AuthService.Application.Contracts;

public interface IJwtService
{
    TokenPair IssueTokens(Guid userId);
}
