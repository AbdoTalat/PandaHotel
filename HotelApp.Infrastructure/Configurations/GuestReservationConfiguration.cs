using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class GuestReservationConfiguration : IEntityTypeConfiguration<GuestReservation>
	{
		public void Configure(EntityTypeBuilder<GuestReservation> builder)
		{
			builder.HasKey(gr => gr.Id);

			builder.HasOne(gr => gr.Guest)
			  .WithMany(g => g.guestReservations)
			  .HasForeignKey(gr => gr.GuestId)
			  .OnDelete(DeleteBehavior.Restrict); 

			builder.HasOne(gr => gr.Reservation)
				   .WithMany(r => r.guestReservations) 
				   .HasForeignKey(gr => gr.ReservationId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.Property(gr => gr.IsPrimaryGuest)
				   .IsRequired();
		}
	}
}