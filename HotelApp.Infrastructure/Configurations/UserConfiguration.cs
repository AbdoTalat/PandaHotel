using HotelApp.Domain.Entities;
using HotelApp.Domain.Entities.RoleBased;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HotelApp.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
            builder.Property(u => u.UserName)
               .HasMaxLength(20);
			builder.Property(u => u.NormalizedUserName)
			   .HasMaxLength(20);

			builder.Property(u => u.Email)
               .HasMaxLength(50);
			builder.Property(u => u.NormalizedEmail)
			   .HasMaxLength(50);

			builder.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(u => u.FirstName)
				.IsRequired()
				.HasMaxLength(20);

			builder.Property(u => u.LastName)
				.IsRequired()
				.HasMaxLength(20);

            builder.Property(u => u.PasswordHash)
                .HasMaxLength(150);

			builder.Property(u => u.SecurityStamp)
			   .HasMaxLength(40);

			builder.Property(u => u.ConcurrencyStamp)
			   .HasMaxLength(40);



			//builder.Ignore(u => u.NormalizedEmail);
   //         builder.Ignore(u => u.NormalizedUserName);
            //builder.Ignore(u => u.SecurityStamp);
            //builder.Ignore(u => u.ConcurrencyStamp);


            builder.HasOne(u => u.DefaultBranch)
			   .WithMany()
			   .HasForeignKey(u => u.DefaultBranchId)
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
