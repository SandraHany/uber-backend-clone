using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Voyage.Application.Abstractions;
using Uber.Voyage.Domain.Sagas;
using Uber.Voyage.Infrastructure.Persistence.Outbox;

namespace Uber.Voyage.Infrastructure.Persistence;

public sealed class VoyageDbContext(DbContextOptions<VoyageDbContext> options) : DbContext(options) , IUnitOfWork
{
    public DbSet<Domain.Entities.AggregateRoots.Voyage> Voyages => Set <Domain.Entities.AggregateRoots.Voyage >();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<VoyageSagaState> VoyageSagaStates => Set<VoyageSagaState>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    => modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoyageDbContext).Assembly);
}
