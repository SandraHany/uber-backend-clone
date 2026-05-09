using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Uber.Sharedkernel.Errors;

namespace Uber.Sharedkernel.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> _validators
    )
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{

    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(validator => validator.ValidateAsync(context)));

        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new Error(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToArray();
        if (errors.Length != 0)
        {
            return (TResponse)typeof(Result)
                .GetMethod(nameof(Result.Failure))!
                .MakeGenericMethod(typeof(TResponse).GenericTypeArguments[0])
                .Invoke(null, [new Errors.ValidationError(errors)])!;
        }
        return await next();

    }
}
