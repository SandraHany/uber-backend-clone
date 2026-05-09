using System;
using System.Collections.Generic;
namespace Uber.Sharedkernel.Errors;

public class Result
{
	protected Result(bool isSuccess, Error error)
	{
		IsSuccess = isSuccess;
		Error = error;

    }
	public Error Error { get; }
    public bool IsSuccess { get; }
	public bool IsFailure => !IsSuccess;
	public static Result Success() => new(true, Error.None);
	public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);


}
public sealed class Result<TValue> : Result
{
	private readonly TValue? _value;
	internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error) => _value = value;

	public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access the value of a failed result.");

    public static implicit operator Result<TValue>(TValue value) => Success(value);
	public static implicit operator Result<TValue>(Error error) => Failure<TValue>(error);
}
