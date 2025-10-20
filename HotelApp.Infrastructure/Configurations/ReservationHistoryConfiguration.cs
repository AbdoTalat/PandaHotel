using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class ReservationHistoryConfiguration : IEntityTypeConfiguration<ReservationHistory>
    {
        public void Configure(EntityTypeBuilder<ReservationHistory> builder)
        {
            builder.HasOne(r => r.Reservation)
                .WithMany(c => c.ReservationHistories)
                .HasForeignKey(r => r.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.PerformedBy)
                .WithMany(c => c.ReservationHistories)
                .HasForeignKey(r => r.PerformedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.ReservationId);
            builder.HasIndex(r => r.PerformedById);
        }
    }
}
