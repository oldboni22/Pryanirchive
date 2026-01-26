using Common.ResultPattern;

namespace FileService.Domain;

public static class DomainErrors
{
    public static readonly Error EmptyFileName = new Error("FileName.Empty", "File name cannot be empty.", ErrorType.Validation);
    
    public static readonly Error InvalidFileName = new Error("FileName.Invalid", "File name is invalid.", ErrorType.Validation);
    
    public static readonly Error TooLargeFileName = new Error("FileName.Large", "File name is too large.", ErrorType.Validation);
    
    public static readonly Error FileExtensionTooLarge = 
        new Error("FileExtension.Large", "File extension is too large.", ErrorType.Validation);
}
