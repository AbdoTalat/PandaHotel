using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelApp.Domain.Entities.RoleBased;

namespace HotelApp.Infrastructure.Configurations.RoleBased
{
    public class EntityConfiguration : IEntityTypeConfiguration<Entity>
	{
		public void Configure(EntityTypeBuilder<Entity> builder)
		{
			builder.Property(e => e.Name)
				.HasMaxLength(20);
		}
	}
}
