using Common.ResultPattern;

namespace FileService.Domain;

public static class FileServiceDomainErrors
{
    public static readonly Error EmptyFileName = new Error("FileName.Empty", "File name cannot be empty.", ErrorType.Validation);
    
    public static readonly Error EmptyBlobId = new Error("BlobId.Empty", "BlobId cannot be empty.", ErrorType.Validation);
    
    public static readonly Error InvalidFileName = new Error("FileName.Invalid", "File name is invalid.", ErrorType.Validation);
    
    public static readonly Error TooLargeFileName = new Error("FileName.Large", "File name is too large.", ErrorType.Validation);
    
    public static readonly Error EmptyFolderName = new Error("FolderName.Empty", "Folder name cannot be empty.", ErrorType.Validation);
    
    public static readonly Error InvalidFolderName = new Error("FolderName.Invalid", "Folder name is invalid.", ErrorType.Validation);
    
    public static readonly Error TooLargeFolderName = new Error("FolderName.Large", "Folder name is too large.", ErrorType.Validation);
    
    public static readonly Error FileExtensionTooLarge = 
        new Error("FileExtension.Large", "File extension is too large.", ErrorType.Validation);
    
    public static readonly Error BlobNotFound = new Error("Blob.NotFound", "Blob Not Found.", ErrorType.NotFound);
    
    public static readonly Error FolderPathTooLarge = 
        new Error("FolderPath.Large", "Folder path is too large.", ErrorType.Validation);
}
