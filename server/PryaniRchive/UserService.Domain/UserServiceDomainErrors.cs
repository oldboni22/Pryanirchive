using Common.ResultPattern;

namespace UserService.Domain;

public static class UserServiceDomainErrors
{
    public static readonly Error NoFileNameError =
        new Error("UserAvatar.EmptyFileName", "The provided file name was empty.", ErrorType.Validation);

    public static readonly Error FileExtensionTooLargeError =
        new Error("UserAvatar.FileExtensionTooLarge", "Avatar file extension is too large.", ErrorType.Validation);
}
