using AuthService.Domain.ValueObjects;
using Common.ResultPattern;

namespace AuthService.Domain.Entities;

public sealed class AuthenticationCredentials
{
    public Guid UserId { get; init; }

    public Email Mail { get; private set; } 

    public HashedString Password { get; private set; }
    
    public void UpdateMail(Email mail) => Mail = mail;
    
    private AuthenticationCredentials() {}

    public static AuthenticationCredentials Create(Guid userId, Email mail, HashedString password)
    {
        return new AuthenticationCredentials
        {
            UserId = userId,
            Password = password,
            Mail = mail
        };
    }
    
    public Result<HashedString> UpdatePassword(string input)
    {
        if (Password.Verify(input))
        {
            return Result.Failure<HashedString>(DomainErrors.RepeatedPassword);
        }

        var newPassword = HashedString.Hash(input);
        
        Password = newPassword;
        return newPassword;
    }
}
