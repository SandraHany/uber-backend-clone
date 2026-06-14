using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Voyage.Domain.Repositories;
using Uber.Voyage.Domain.Sagas;

namespace Uber.Voyage.Infrastructure.Persistence.Repositories;

internal sealed class VoyageSagaRepository(VoyageDbContext dbContext) : IVoyageSagaRepository
{
    public async Task<VoyageSagaState> AddAsync(VoyageSagaState voyageSagaState, CancellationToken ct = default)
    {
        var result = await dbContext.VoyageSagaStates.AddAsync(voyageSagaState);
        return result.Entity;
       
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    {
       return await dbContext.VoyageSagaStates.AnyAsync(v => v.Id == id);
    }

    public async Task<VoyageSagaState?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
       return await dbContext.VoyageSagaStates.FirstOrDefaultAsync(v => v.Id == id, ct);
    }
}
