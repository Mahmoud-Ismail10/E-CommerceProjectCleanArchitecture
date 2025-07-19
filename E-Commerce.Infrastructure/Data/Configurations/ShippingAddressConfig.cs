using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class ShippingAddressConfig : IEntityTypeConfiguration<ShippingAddress>
    {
        public void Configure(EntityTypeBuilder<ShippingAddress> builder)
        {
            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Street)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.City)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.State)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasOne(s => s.Customer)
                .WithMany(u => u.ShippingAddresses)
                .HasForeignKey(s => s.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
