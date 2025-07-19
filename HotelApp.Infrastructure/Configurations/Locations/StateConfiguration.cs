using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities;

namespace HotelApp.Infrastructure.Configurations.Locations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
	{
		public void Configure(EntityTypeBuilder<State> builder)
		{
			builder.Property(s => s.Id)
				.ValueGeneratedNever();

			builder.Property(s => s.Name)
				.HasMaxLength(40)
				.IsRequired();

			builder.HasOne(s => s.Country)
				.WithMany(c => c.States)
				.HasForeignKey(s => s.CountryId)
				.OnDelete(DeleteBehavior.Cascade);


		}
	}
}
