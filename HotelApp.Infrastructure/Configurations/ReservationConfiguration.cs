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
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
	{
		public void Configure(EntityTypeBuilder<Reservation> builder)
		{
			builder.Property(b => b.CheckInDate)
			   .IsRequired();

			builder.Property(b => b.CheckOutDate)
			   .IsRequired();

			builder.Property(b => b.NumberOfNights)
				.IsRequired();

			builder.Property(r => r.Comment)
				.HasMaxLength(200);

			builder.Property(b => b.CancellationReason)
			   .HasMaxLength(200);

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
		}
	}
}
