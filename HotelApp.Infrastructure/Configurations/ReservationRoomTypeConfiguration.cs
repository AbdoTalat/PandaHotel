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
	public class ReservationRoomTypeConfiguration : IEntityTypeConfiguration<ReservationRoomType>
	{
		public void Configure(EntityTypeBuilder<ReservationRoomType> builder)
		{
			builder.HasOne(rrt => rrt.RoomType)
				.WithMany(rt => rt.ReservationRoomTypes)
				.HasForeignKey(rt => rt.RoomTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(rrt => rrt.Reservation)
				.WithMany(rt => rt.ReservationRoomTypes)
				.HasForeignKey(rt => rt.ReservationId)
				.OnDelete(DeleteBehavior.Restrict);


		}
	}
}
