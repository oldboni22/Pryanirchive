using System.Buffers;
using Common.ResultPattern;

namespace Common.Data;

public abstract record EntityName
{
    protected EntityName(string value) => Value = value;
    
    public string Value { get; init; }

    public override string ToString() => Value;
    
    protected static Result ValidateName(
        string value, ushort maxLength, ushort minLength = 0, SearchValues<char>? forbiddenChars = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return NamedEntityErrors.EmptyNameError;
        }

        if (forbiddenChars is not null && value.AsSpan().ContainsAny(forbiddenChars))
        {
            return NamedEntityErrors.InvalidNameError;
        }

        var length = value.Length;

        if (length > maxLength)
        {
            return NamedEntityErrors.LargeNameError;
        }

        if (length < minLength)
        {
            return NamedEntityErrors.ShortNameError;
        }

        return Result.Success();
    }
}
