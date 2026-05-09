using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Uber.Trip.Infrastructure.Persistence.Configurations
{
    internal sealed class TripConfiguration : IEntityTypeConfiguration<Domain.Entities.Trip>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Trip> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.RiderId);
            builder.Property(u => u.Status)
                    .HasConversion<string>();
            builder.OwnsOne(t => t.PickupLocation, location =>
            {
                location.Property(l => l.Latitute)
                    .HasColumnName("pickup_latitude");
                location.Property(l => l.Longitude)
                    .HasColumnName("pickup_longitude");

            });
            builder.OwnsOne(t => t.DropoffLocation, location =>
            {
                location.Property(l => l.Latitute)
                    .HasColumnName("droppff_latitude");
                location.Property(l => l.Longitude)
                    .HasColumnName("dropoff_longitude");
            });
            builder.ToTable("trips");

        }
    }
}
