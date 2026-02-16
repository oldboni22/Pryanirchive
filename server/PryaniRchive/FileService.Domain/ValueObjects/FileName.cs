using System.Buffers;
using Common.Data;
using Common.ResultPattern;

namespace FileService.Domain.ValueObjects;

public sealed record FileName : EntityName
{
    public const int MaxNameLength = 60;
    
    private static readonly SearchValues<char> InvalidCharsSearch = 
        SearchValues.Create(Path.GetInvalidFileNameChars());

    private FileName(string value) : base(value) { }
    
    public string Name => Path.GetFileNameWithoutExtension(Value);
    public string? Extension => Path.GetExtension(Value);
    
    public static Result<FileName> Create(string name)
    {
        var validationResult = ValidateName(name, MaxNameLength, forbiddenChars: InvalidCharsSearch);

        if (validationResult.IsSuccess) 
        {
            return new FileName(name);
        }

        return Result<FileName>.FromFailedResult(validationResult);
    }

    public static FileName FromDatabase(string value) => new FileName(value);
}
