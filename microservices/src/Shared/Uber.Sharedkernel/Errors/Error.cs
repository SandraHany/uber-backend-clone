using System;
namespace Uber.Sharedkernel.Errors;

public class Error
{
    public string Code { get; }
    public string Description { get; }
    public Error(string code, string description)
    {
        Code = code;
        Description = description;

    }
    public static implicit operator Result(Error error) => Result.Failure(error);
    public static readonly Error None = new(string.Empty, string.Empty);


}
