using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Sharedkernel.Errors;
using Uber.Trip.Application.Abstractions;
using Uber.Trip.Domain.Repositories;

namespace Uber.Trip.Application.Features.RequestTrip;

internal sealed class RequestTripCommandHandler(
    ITripRepository tripRepository, IUnitOfWork unitOfWork
    ) : IRequestHandler<RequestTripCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(RequestTripCommand request, CancellationToken cancellationToken)
    {
        var trip = Domain.Entities.Trip.Create(request.RiderId, request.pickupLocation, request.dropoffLocation);

        tripRepository.Add(trip);
        await unitOfWork.SaveChangesAsync();
        return trip.Id;
    }
}
