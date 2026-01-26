using Common.ResultPattern;

namespace Common.Blob;

public static class BlobErrors
{
    public static readonly Error BlobNotFound = new Error("Blob.NotFound", "Blob Not Found.", ErrorType.NotFound);
}
