using Common.ResultPattern;

namespace AuthService.Domain;

public static class DomainErrors
{
    public static readonly Error InvalidEmail = new Error("Email.Invalid", "Email is invalid.", ErrorType.Validation);
    
    public static readonly Error RepeatedPassword = new Error("Password.Repeated", "Password is repeated.", ErrorType.Validation);
    
    public static readonly Error InvalidAccessToken = new Error("AccessToken.Invalid", "Access token is invalid.", ErrorType.Validation);
    
    public static readonly Error InvalidRefreshToken = new Error("RefreshToken.Invalid", "Refresh token is invalid.", ErrorType.Validation);
}
