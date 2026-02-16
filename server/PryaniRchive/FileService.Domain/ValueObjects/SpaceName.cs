using Common.Data;
using Common.ResultPattern;

namespace FileService.Domain.ValueObjects;

public sealed record SpaceName : EntityName
{
    public const ushort MaxNameLength = 60;

    private SpaceName(string value) : base(value) { }
    
    public static Result<SpaceName> Create(string name)
    {
        var validationResult = ValidateName(name, MaxNameLength);

        if (validationResult.IsSuccess)
        {
            return new SpaceName(name);
        }

        return Result<SpaceName>.FromFailedResult(validationResult);
    }

    public static SpaceName FromDatabase(string value) => new SpaceName(value);
}