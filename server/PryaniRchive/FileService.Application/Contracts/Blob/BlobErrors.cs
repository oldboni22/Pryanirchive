using Common.ResultPattern;

namespace FileService.Application.Contracts.Blob;

public static class BlobErrors
{
    public static readonly Error BlobNotFound = new Error("Blob.NotFound", "Blob Not Found.", ErrorType.NotFound);
}
