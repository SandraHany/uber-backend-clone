using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Sharedkernel.Errors;

namespace Uber.Trip.Application.Features.RequestTrip
{
    public sealed record RequestTripCommand(
        Guid RiderId,
        Location pickupLocation,
        Location dropoffLocation
        ) : IRequest<Result<Guid>>;

 

}
