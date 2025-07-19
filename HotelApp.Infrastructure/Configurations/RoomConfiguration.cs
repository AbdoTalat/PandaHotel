using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;

namespace HotelApp.Infrastructure.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
	{
		public void Configure(EntityTypeBuilder<Room> builder)
		{
			builder.Property(b => b.RoomNumber)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(r => r.Description)
				.IsRequired()
				.HasMaxLength(500);

			builder.Property(r => r.PricePerNight)
				.IsRequired();


			builder.HasOne(r => r.RoomType)
				.WithMany(r => r.Rooms)
				.HasForeignKey(r => r.RoomTypeId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(r => r.Branch)
				.WithMany(b => b.Rooms)
				.HasForeignKey(r => r.BranchId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(r => r.RoomStatus)
				.WithMany(rs => rs.Rooms)
				.HasForeignKey(r => r.RoomStatusId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(r => r.Floor)
				.WithMany(f => f.Rooms)
				.HasForeignKey(r => r.FloorId)
				.OnDelete(DeleteBehavior.Restrict);


			builder.HasOne(b => b.CreatedBy)
				.WithMany()
				.HasForeignKey(b => b.CreatedById)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(b => b.LastModifiedBy)
				.WithMany()
				.HasForeignKey(b => b.LastModifiedById)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasIndex(r => new { r.RoomNumber, r.BranchId })
				.IsUnique();
		}
	}
}
