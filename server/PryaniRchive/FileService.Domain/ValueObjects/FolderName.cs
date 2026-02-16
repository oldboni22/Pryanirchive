using Common.Data;
using Common.ResultPattern;

namespace FileService.Domain.ValueObjects;

public sealed record FolderName : EntityName
{
    public const ushort MaxNameLength = 60;

    private FolderName(string value) : base(value) { }
    
    public static Result<FolderName> Create(string name)
    {
        var validationResult = ValidateName(name, MaxNameLength);

        if (validationResult.IsSuccess)
        {
            return new FolderName(name);
        }

        return Result<FolderName>.FromFailedResult(validationResult);
    }

    public static FolderName FromDatabase(string value) => new FolderName(value);
}