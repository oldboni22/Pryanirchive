namespace Common.ResultPattern;

public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static Error None => new Error(string.Empty, string.Empty, ErrorType.None);
}
