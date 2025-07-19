using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class GuestConfiguration : IEntityTypeConfiguration<Guest>
	{
		public void Configure(EntityTypeBuilder<Guest> builder)
		{
			builder.Property(b => b.FirstName)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.MiddleName)
				.HasMaxLength(50);

			builder.Property(b => b.LastName)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.Email)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.Address)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.Phone)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.TypeOfProof)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(b => b.ProofNumber)
				.IsRequired()
				.HasMaxLength(20);

			builder.HasOne(g => g.Branch)
				.WithMany(b => b.Guests)
				.HasForeignKey(g => g.BranchId)
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
