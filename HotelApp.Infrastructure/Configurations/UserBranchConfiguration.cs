using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class UserBranchConfiguration : IEntityTypeConfiguration<UserBranch>
	{
		public void Configure(EntityTypeBuilder<UserBranch> builder)
		{
			builder.HasKey(ub => ub.Id);

			//builder.Property(ub => ub.UserId)
			//	.HasMaxLength(40);


			builder.HasOne(ub => ub.User)
				.WithMany(u => u.userBranches)
				.HasForeignKey(u => u.UserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(ub => ub.Branch)
				.WithMany(u => u.userBranches)
				.HasForeignKey(u => u.BranchId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.Property(ub => ub.AssignedAt)
				.IsRequired();
		}
	}
}
