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
    public class RoomTypeConfiguration : IEntityTypeConfiguration<RoomType>
	{
		public void Configure(EntityTypeBuilder<RoomType> builder)
		{
			builder.Property(r => r.Name)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(r => r.Description)
				.HasMaxLength(400);

			builder.Property(r => r.PricePerNight)
				.IsRequired()
				.HasColumnType("decimal(18,2)");

			builder.Property(rt => rt.MaxNumOfAdults)
				.IsRequired()
				.HasMaxLength (20);

            builder.Property(rt => rt.MaxNumOfChildrens)
                .IsRequired()
                .HasMaxLength(20);
			
			builder.HasOne(rs => rs.Branch)
                .WithMany(b => b.RoomTypes)
                .HasForeignKey(rs => rs.BranchId)
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
