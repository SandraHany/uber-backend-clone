using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Uber.Voyage.Domain.Sagas;

namespace Uber.Voyage.Infrastructure.Persistence.Configurations;

internal sealed class VoyageSagaStateConfiguration : IEntityTypeConfiguration<VoyageSagaState>
{
    public void Configure(EntityTypeBuilder<VoyageSagaState> builder)
    {
        builder.ToTable("VoyageSagaStates");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.PassengerId)
            .IsRequired();

        builder.Property(v => v.DriverId);

        builder.Property(v => v.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.VehicleType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(v => v.CreatedAtUtc)
            .IsRequired();

        builder.Property(v => v.CompletedAtUtc);

        builder.OwnsOne(v => v.Pickup, pickup =>
        {
            pickup.OwnsOne(l => l.Coordinates, coords =>
            {
                coords.Property(c => c.Latitude)
                    .HasColumnName("pickup_latitude")
                    .IsRequired();

                coords.Property(c => c.Longitude)
                    .HasColumnName("pickup_longitude")
                    .IsRequired();
            });

            pickup.Property(l => l.Address)
                .HasColumnName("pickup_address")
                .HasMaxLength(500);
        });

        builder.OwnsOne(v => v.Dropoff, dropoff =>
        {
            dropoff.OwnsOne(l => l.Coordinates, coords =>
            {
                coords.Property(c => c.Latitude)
                    .HasColumnName("dropoff_latitude")
                    .IsRequired();

                coords.Property(c => c.Longitude)
                    .HasColumnName("dropoff_longitude")
                    .IsRequired();
            });

            dropoff.Property(l => l.Address)
                .HasColumnName("dropoff_address")
                .HasMaxLength(500);
        });

        builder.Property(x => x.FailureReason)
            .HasMaxLength(1000)
            .IsRequired(false);

        builder.Property(x => x.Version)
            .IsConcurrencyToken();
    }
}