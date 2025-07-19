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
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
	{
		public void Configure(EntityTypeBuilder<Branch> builder)
		{
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.Street)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.City)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.ContactNumber)
			   .IsRequired()
			   .HasMaxLength(20);

			builder.Property(b => b.Zip_Code)
				.IsRequired()
				.HasMaxLength(20);



			builder.HasOne(b => b.Country)
				.WithMany(c => c.Branches)
				.HasForeignKey(b => b.CountryId)
				.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.State)
                .WithMany(c => c.Branches)
                .HasForeignKey(b => b.StateId)
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
