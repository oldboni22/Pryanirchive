using Common.Data;
using Common.ResultPattern;

namespace UserService.Domain.ValueObjects;

public sealed record UserName : EntityName
{
    public const ushort MaxNameLength = 25;
    
    private const ushort MinNameLength = 5;

    private UserName(string value) : base(value) { }
    
    public static Result<UserName> Create(string name)
    {
        var validationResult = ValidateName(name, MaxNameLength, MinNameLength);

        if (validationResult.IsSuccess)
        {
            return new UserName(name);
        }

        return Result<UserName>.FromFailedResult(validationResult);
    }

    public static UserName FromDatabase(string value) => new UserName(value);
}
