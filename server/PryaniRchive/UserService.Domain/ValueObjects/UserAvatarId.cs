using Common.ResultPattern;

namespace UserService.Domain.ValueObjects;

public record UserAvatarId
{
    private const int GuidLength = 36;

    private const int MaxTicksLength = 20;
    
    private const string Prefix = "avatar";
    
    private const int MaxFileExtensionLength = 10;

    public const int MaxFieldLength = GuidLength + MaxFileExtensionLength + 8 + MaxTicksLength;
    
    public string Value { get; private set; }

    private UserAvatarId(string value) => Value = value;

    public static Result<UserAvatarId> Create(string fileName, Guid userId, DateTime createdOn)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return UserServiceDomainErrors.NoFileNameError;
        }

        var ext = Path.GetExtension(fileName);

        if (ext.Length is > MaxFileExtensionLength or <= 1)
        {
            return UserServiceDomainErrors.FileExtensionTooLargeError;
        }

        var ticks = createdOn.Ticks;
        
        return new UserAvatarId($"{Prefix}_{userId}_{ticks}{ext}");
    }

    public override string ToString() => Value;

    public static implicit operator string(UserAvatarId avatarId) => avatarId.Value; 
    
    public static UserAvatarId FromDatabase(string? value) => new UserAvatarId(value);
};
