using System.Net.Mail;

namespace AuthService.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; init; }
    
    private Email(string value) => Value = value;

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
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
