using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uber.Trip.Application.Features.RequestTrip;

internal sealed class RequestCommandValidator : AbstractValidator<RequestTripCommand>
{
  public RequestCommandValidator(){
        RuleFor(x => x.RiderId).NotEmpty();
    }
}
