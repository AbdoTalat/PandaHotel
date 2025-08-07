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
			builder.Property(b => b.FullName)
				.IsRequired()
				.HasMaxLength(60);

			builder.Property(b => b.Email)
				.HasMaxLength(50);

			builder.Property(b => b.Address)
				.HasMaxLength(50);

			builder.Property(b => b.Phone)
				.HasMaxLength(20);

			builder.Property(b => b.TypeOfProof)
				.HasMaxLength(20);

			builder.Property(b => b.ProofNumber)
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
