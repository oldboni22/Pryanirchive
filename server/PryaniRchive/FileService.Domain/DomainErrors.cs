using Common.ResultPattern;

namespace FileService.Domain;

public static class DomainErrors
{
    public static Error EmptyFileName => new Error("FileName.Empty", "File name cannot be empty.", ErrorType.Validation);
    public static Error InvalidFileName => new Error("FileName.Invalid", "File name is invalid.", ErrorType.Validation);
    public static Error TooLargeFileName => new Error("FileName.Large", "File name is too large.", ErrorType.Validation);
}
