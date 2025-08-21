using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelApp.Infrastructure.Configurations
{
    public class FloorConfiguration : IEntityTypeConfiguration<Floor>
	{
		public void Configure(EntityTypeBuilder<Floor> builder)
		{

            builder.HasOne(f => f.Branch)
				.WithMany(b => b.Floors)
				.HasForeignKey(f => f.BranchId)
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
