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
    public class MasterDataItemConfiguration : IEntityTypeConfiguration<MasterDataItem>
    {
        public void Configure(EntityTypeBuilder<MasterDataItem> builder)
        {
            builder.HasKey(ddi => ddi.Id);

            builder.Property(ddi => ddi.Value)
                .IsRequired();

            builder.Property(ddi => ddi.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasOne(ddi => ddi.MasterDataType)
               .WithMany(ddt => ddt.MasterDataItems)
               .HasForeignKey(ddi => ddi.MasterDataTypeId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
