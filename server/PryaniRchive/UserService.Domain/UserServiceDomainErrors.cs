using Common.ResultPattern;

namespace UserService.Domain;

public static class UserServiceDomainErrors
{
    public static readonly Error EmptyUserId = new Error("UserId.Empty", "UserId cannot be empty.", ErrorType.Validation);
    
    public static readonly Error EmptyAvatarId = new Error("AvatarId.Empty", "AvatarId cannot be empty.", ErrorType.Validation);
    
    public static readonly Error NoFileNameError =
        new Error("UserAvatar.EmptyFileName", "The provided file name was empty.", ErrorType.Validation);

    public static readonly Error FileExtensionTooLargeError =
        new Error("UserAvatar.FileExtensionTooLarge", "Avatar file extension is too large.", ErrorType.Validation);
    
    public static readonly Error EmptyFileExtensionError =
        new Error("UserAvatar.NoFileExtension", "Avatar file extension is missing.", ErrorType.Validation);
    
    public static readonly Error ShortUserNameError =
        new Error("User.TooShortName", "The provided username is invalid.", ErrorType.Validation);
    
    public static readonly Error LongUserNameError =
        new Error("User.TooLargeName", "The provided username is invalid.", ErrorType.Validation);
}
