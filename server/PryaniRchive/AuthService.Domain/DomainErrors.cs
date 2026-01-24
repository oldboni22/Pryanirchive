using Common.ResultPattern;

namespace AuthService.Domain;

public static class DomainErrors
{
    public static Error InvalidEmail => new Error("Email.Invalid", "Email is invalid.", ErrorType.Validation);
    
    public static Error RepeatedPassword => new Error("Password.Repeated", "Password is repeated.", ErrorType.Validation);
}
