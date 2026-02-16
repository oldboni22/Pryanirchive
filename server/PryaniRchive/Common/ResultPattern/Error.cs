namespace Common.ResultPattern;

public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static Error None => new Error(string.Empty, string.Empty, ErrorType.None);
    
    public static Error NotFound => new Error("Resource.NorFound", "The resource was not found.", ErrorType.NotFound);
    
    public static Error Collision => new Error("Resource.Collision", "The resource already exists", ErrorType.Collision);
}
