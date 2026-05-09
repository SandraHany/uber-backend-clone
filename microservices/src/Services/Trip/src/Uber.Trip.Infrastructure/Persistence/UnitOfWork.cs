using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using Uber.Trip.Application.Abstractions;
namespace Uber.Trip.Infrastructure.Persistence;

public sealed class UnitOfWork(TripDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await context.SaveChangesAsync(ct);
    }


}
