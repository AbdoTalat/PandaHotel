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
    public class RoomOptionConfiguration : IEntityTypeConfiguration<RoomOption>
 	{
		public void Configure(EntityTypeBuilder<RoomOption> builder)
		{
			builder.HasKey(ro => ro.Id);

			builder.HasOne(ro => ro.Room)
				.WithMany(r => r.RoomOptions)
				.HasForeignKey(ro => ro.RoomId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(ro => ro.Option)
				.WithMany(o => o.RoomOptions)
				.HasForeignKey(ro => ro.OptionId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
