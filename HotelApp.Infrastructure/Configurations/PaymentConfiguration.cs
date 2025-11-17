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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.Notes)
                .HasMaxLength(50);

            builder.HasOne(p => p.Reservation)
                .WithMany(r => r.Payments)
                .HasForeignKey(p => p.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Guest)
                .WithMany(g => g.Payments)
                .HasForeignKey(p => p.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Branch)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.TransactionType)
                .WithMany()
                .HasForeignKey(p => p.TransactionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.PaymentMethod)
                .WithMany()
                .HasForeignKey(p => p.PaymentMethodId)
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
