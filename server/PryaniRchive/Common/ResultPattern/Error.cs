using System;

namespace Common.ResultPattern;

public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static Error None => new Error(string.Empty, string.Empty, ErrorType.None);
    
    public static Error NotFound => new Error("Resource.NorFound", "The resource was not found.", ErrorType.NotFound);
    
    public static Error Exception(Exception exception) => new Error("Exception", "An internal infrastructure error occurred.", ErrorType.Exception);
}
