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
    public class OptionConfiguration : IEntityTypeConfiguration<Option>
	{
		public void Configure(EntityTypeBuilder<Option> builder)
		{
			builder.Property(o => o.Name)
				.HasMaxLength(50)
				.IsRequired();

            builder.Property(o => o.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(o => o.HourlyPrice)
               .HasColumnType("decimal(18,2)");

            builder.Property(o => o.DailyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.ExtraDailyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.WeeklyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.MonthlyPrice)
                .HasColumnType("decimal(18,2)");

            //builder.Property(o => o.BranchId)
            //    .HasDefaultValue(2);

            builder.HasOne(o => o.Branch)
                .WithMany(b => b.Options)
                .HasForeignKey(o => o.BranchId)
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
