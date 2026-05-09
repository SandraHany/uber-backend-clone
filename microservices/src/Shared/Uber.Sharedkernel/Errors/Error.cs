using System;
namespace Uber.Sharedkernel.Errors;

public class Error(string Code, string Description)
{
    public static implicit operator Result(Error error) => Result.Failure(error);
    public static readonly Error None = new(string.Empty, string.Empty);


}
