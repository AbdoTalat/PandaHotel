using HotelApp.Domain.Entities;
using HotelApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
	{
		public void Configure(EntityTypeBuilder<Reservation> builder)
		{
            builder.Property(r => r.ReservationNumber)
                .HasMaxLength(30);

            builder.Property(b => b.CheckInDate)
			   .IsRequired();

			builder.Property(b => b.CheckOutDate)
			   .IsRequired();

			builder.Property(b => b.NumberOfNights)
				.IsRequired();

			builder.Property(r => r.Notes)
				.HasMaxLength(100);

			builder.Property(b => b.CancellationReason)
			   .HasMaxLength(50);

			builder.Property(r => r.Status)
				.HasConversion<int>();
			//	.HasDefaultValue((int)ReservationStatus.Pending);


			builder.Property(r => r.TotalPrice)
				.HasColumnType("decimal(18,2)");

			builder.Property(r => r.PricePerNight)
				.HasColumnType("decimal(18,2)");

			builder.HasOne(r => r.Branch)
				.WithMany(b => b.Reservations)
				.HasForeignKey(r => r.BranchId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(r => r.ReservationSource)
				.WithMany(rs => rs.Reservations)
				.HasForeignKey(r => r.ReservationSourceId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(r => r.Company)
				.WithMany(c => c.Reservations)
				.HasForeignKey(r => r.CompanyId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.CreatedBy)
			   .WithMany()
			   .HasForeignKey(b => b.CreatedById)
			   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.LastModifiedBy)
				.WithMany()
				.HasForeignKey(b => b.LastModifiedById)
				.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Rate)
               .WithMany()
               .HasForeignKey(b => b.RateId)
               .OnDelete(DeleteBehavior.Restrict);
        }
	}
}
