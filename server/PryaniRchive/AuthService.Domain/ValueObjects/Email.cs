using System.Net.Mail;
using Common.ResultPattern;

namespace AuthService.Domain.ValueObjects;

public sealed record Email
{
    public const int MaxLength = 255;
    
    public string Value { get; init; }
    
    private Email(string value) => Value = value;

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) &&  value.Length > MaxLength)
        {
            return Result.Failure<Email>(DomainErrors.InvalidEmail);
        }
        try
        {
            var mail = new MailAddress(value);

            return new Email(mail.Address.ToLowerInvariant());
        }
        catch (FormatException)
        {
            return Result.Failure<Email>(DomainErrors.InvalidEmail);
        }
    }
    
    public override string ToString() => Value;
    
    public static implicit operator string(Email email) => email.Value;

    public static Email FromDatabase(string value) => new Email(value);
}
