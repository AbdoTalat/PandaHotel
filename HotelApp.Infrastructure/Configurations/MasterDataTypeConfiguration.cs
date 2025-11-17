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
    public class MasterDataTypeConfiguration : IEntityTypeConfiguration<MasterDataType>
    {
        public void Configure(EntityTypeBuilder<MasterDataType> builder)
        {
            builder.HasKey(dt => dt.Id);

            builder.Property(dt => dt.Name)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
