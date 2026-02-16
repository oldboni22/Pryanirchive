using Common.ResultPattern;

namespace Common.Data;

public static class NamedEntityErrors
{
    public static readonly Error LargeNameError = 
        new Error("LargeName", "The name is too large", ErrorType.Validation);
    
    public static readonly Error ShortNameError = 
        new Error("ShortName", "The name is too short", ErrorType.Validation);
    
    public static readonly Error EmptyNameError = 
        new Error("EmptyName", "The name is empty", ErrorType.Validation);
    
    public static readonly Error InvalidNameError = 
        new Error("InvalidName", "The name is invalid", ErrorType.Validation);
}
