using HotelApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Infrastructure.Configurations
{
    public class ProofTypeConfiguration : IEntityTypeConfiguration<ProofType>
    {
        public void Configure(EntityTypeBuilder<ProofType> builder)
        {
            builder.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(40);

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
