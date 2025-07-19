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
    public class RoomTypeRateConfiguration : IEntityTypeConfiguration<RoomTypeRate>
    {
        public void Configure(EntityTypeBuilder<RoomTypeRate> builder)
        {
            builder.HasKey(rr => rr.Id);

            builder.Property(rtr => rtr.HourlyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(rtr => rtr.DailyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(rtr => rtr.ExtraDailyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(rtr => rtr.WeeklyPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(rtr => rtr.MonthlyPrice)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(rr => rr.RoomType)
                .WithMany(rt => rt.RoomTypeRates)
                .HasForeignKey(rt => rt.RoomTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rr => rr.Rate)
                .WithMany(rt => rt.RoomTypeRates)
                .HasForeignKey(rt => rt.RateId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
