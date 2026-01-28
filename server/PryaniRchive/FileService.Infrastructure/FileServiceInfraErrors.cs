using Common.ResultPattern;

namespace FileService.Infrastructure;

public static class FileServiceInfraErrors
{
    public static readonly Error NotAllowedAction = 
        new Error("User.NotAllowed", "The user is not allowed to perform that action.", ErrorType.NotAllowed);
}
