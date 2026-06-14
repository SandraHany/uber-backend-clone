using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Voyage.Domain.Repositories;

namespace Uber.Voyage.Infrastructure.Persistence.Repositories;

public class VoyageRepository : IVoyageRepository
{
    private readonly VoyageDbContext _dbContext;
    public VoyageRepository(VoyageDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Domain.Entities.AggregateRoots.Voyage voyage, CancellationToken ct = default)
    => await _dbContext.Voyages.AddAsync(voyage, ct);

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
    => await _dbContext.Voyages.AnyAsync(v => v.Id == id, ct);

    public async Task<Domain.Entities.AggregateRoots.Voyage?> GetByIdAsync(Guid id, CancellationToken ct = default)
    => await _dbContext.Voyages.FirstOrDefaultAsync(v => v.Id == id, ct);

    public async Task UpdateAsync(Domain.Entities.AggregateRoots.Voyage voyage, CancellationToken ct = default)
    {
        _dbContext.Voyages.Update(voyage);
        await Task.CompletedTask;
    }
}
