using System.Buffers;
using Common.ResultPattern;

namespace FileService.Domain.ValueObjects;

public sealed record FileName
{
    public const int MaxFileNameLength = 60;
    
    private static readonly SearchValues<char> InvalidCharsSearch = 
        SearchValues.Create(Path.GetInvalidFileNameChars());
    
    public string Value { get; private set; }
    
    public string Name => Path.GetFileNameWithoutExtension(Value);
    public string? Extension => Path.GetExtension(Value);
    
    private FileName(string value) => Value = value; 
    
    public static Result<FileName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result.Failure<FileName>(DomainErrors.EmptyFileName);
        }

        if (value.AsSpan().ContainsAny(InvalidCharsSearch))
        {
            return Result.Failure<FileName>(DomainErrors.InvalidFileName);
        }

        if (value.Length > MaxFileNameLength)
        {
            return Result.Failure<FileName>(DomainErrors.TooLargeFileName);
        }
        
        if (Path.HasExtension(value))
        {
            var extensionToLower = Path.GetExtension(value).ToLower();
            
            return new FileName($"{Path.GetFileNameWithoutExtension(value)}{extensionToLower}");
        }

        return new FileName(value);
    }
    
    public static implicit operator string (FileName fileName) => fileName.Value;

    public static FileName FromDatabase(string value) => new FileName(value);
}
