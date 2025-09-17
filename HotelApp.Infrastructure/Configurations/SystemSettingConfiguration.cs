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
	public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
	{
		public void Configure(EntityTypeBuilder<SystemSetting> builder)
		{
			builder.HasOne(ss => ss.CheckInStatus)
				.WithMany()
				.HasForeignKey(ss => ss.CheckInStatusId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(ss => ss.CheckOutStatus)
				.WithMany()
				.HasForeignKey(ss => ss.CheckOutStatusId)
				.OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.CalculationType)
               .WithMany()
               .HasForeignKey(ss => ss.CalculationTypeId)
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
