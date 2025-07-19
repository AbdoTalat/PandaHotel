using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations.Locations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.Property(c => c.Id)
				.ValueGeneratedNever();

			builder.Property(c => c.ShortName)
				.HasMaxLength(5)
				.IsRequired();

			builder.Property(c => c.Name)
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(c => c.PhoneCode)
				.HasMaxLength(15)
				.IsRequired();
		}
	}
}
