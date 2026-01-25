using System.Security.Cryptography;

namespace UserService.Domain.ValueObjects;

public record UserTag
{
    private const int Size = 8;
    
    public string Value { get; init; }
    
    private UserTag() {}

    public static Result<UserTag> Create()
    {
        var bytes = RandomNumberGenerator.GetBytes(Size / 2);
        return new UserTag
        {
            Value = Convert.ToHexString(bytes).ToLower()
        };
    }

    public static UserTag FromDatabase(string value) => new UserTag {Value = value};
    
    public override string ToString() => Value;
    
    public static implicit operator string(UserTag tag) => tag.Value;
}
