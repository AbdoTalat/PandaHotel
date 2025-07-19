using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.Property(r => r.Code)
                .HasMaxLength(50)
                .IsRequired();

			builder.HasIndex(r => new { r.Code, r.BranchId })
                .IsUnique();

            builder.HasOne(r => r.Branch)
                .WithMany(b => b.Rates)
                .HasForeignKey(r => r.BranchId)
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
