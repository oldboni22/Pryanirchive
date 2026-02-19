using Common.ResultPattern;

namespace UserService.Domain.ValueObjects;

public record UserAvatarId
{
    private const int GuidLength = 36;
    
    private const int GuidNLength = 32;
    
    private const int SeparatorsLength = 2;
    
    private const int PrefixLength = 6;
    
    private const int MaxFileExtensionLength = 10;
    
    public const int MaxFieldLength = GuidLength + GuidNLength + MaxFileExtensionLength + PrefixLength + SeparatorsLength;
    
    private const string Prefix = "avatar";
    
    public string Value { get; private set; }

    private UserAvatarId(string value) => Value = value;

    public static Result<UserAvatarId> Create(string fileName, Guid userId, Guid generatedId)
    {
        if (userId == Guid.Empty)
        {
            return UserServiceDomainErrors.EmptyUserId;
        }

        if (generatedId == Guid.Empty)
        {
            return UserServiceDomainErrors.EmptyAvatarId;
        }
        
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return UserServiceDomainErrors.NoFileNameError;
        }

        var ext = Path.GetExtension(fileName);

        if (ext.Length is > MaxFileExtensionLength)
        {
            return UserServiceDomainErrors.FileExtensionTooLargeError;
        }
        else if (string.IsNullOrEmpty(ext) || ext == ".")
        {
            return UserServiceDomainErrors.EmptyFileExtensionError;
        }
        
        return new UserAvatarId($"{Prefix}_{userId}_{generatedId:N}{ext}");
    }

    public override string ToString() => Value;

    public static implicit operator string(UserAvatarId avatarId) => avatarId.Value; 
    
    public static UserAvatarId? FromDatabase(string? value) => value is null ? null : new UserAvatarId(value);
}
