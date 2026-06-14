using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uber.Voyage.Domain.Enums;

namespace Uber.Voyage.Infrastructure.Persistence.Configurations;

public sealed class VoyageConfiguration : IEntityTypeConfiguration<Domain.Entities.AggregateRoots.Voyage>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.AggregateRoots.Voyage> builder)
    {
        builder.ToTable("voyages");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.RiderId)
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
    }
}