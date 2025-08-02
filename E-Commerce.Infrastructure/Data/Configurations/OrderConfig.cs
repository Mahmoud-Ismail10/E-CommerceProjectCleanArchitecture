using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.OrderDate)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(o => o.Status)
                .HasConversion(
                Sts => Sts.ToString(),
                Sts => (Status)Enum.Parse(typeof(Status), Sts))
                .IsRequired();

            builder.Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.ShippingAddress)
                .WithOne()
                .HasForeignKey<Order>(o => o.ShippingAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Payment)
                .WithOne()
                .HasForeignKey<Order>(o => o.PaymentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Delivery)
                .WithOne()
                .HasForeignKey<Order>(o => o.DeliveryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
