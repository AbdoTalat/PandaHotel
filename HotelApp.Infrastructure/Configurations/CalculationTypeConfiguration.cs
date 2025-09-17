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
    public class CalculationTypeConfiguration : IEntityTypeConfiguration<CalculationType>
    {
        public void Configure(EntityTypeBuilder<CalculationType> builder)
        {
            builder.Property(ct => ct.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
