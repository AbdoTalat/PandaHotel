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
    public class RoomStatusConfiguration : IEntityTypeConfiguration<RoomStatus>
    {
        public void Configure(EntityTypeBuilder<RoomStatus> builder)
        {
            builder.Property(rs => rs.Name)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(rs => rs.Code)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(rs => rs.Description)
                .HasMaxLength(200);

            builder.Property(rs => rs.Color)
                .IsRequired()
                .HasMaxLength(8);

            builder.HasOne(rs => rs.Branch)
                .WithMany(b => b.RoomStatuses)
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
