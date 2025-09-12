using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Configure TPT
            builder.ToTable("Customers");

            builder.Property(a => a.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.LastName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Gender)
                .HasConversion(
                Gndr => Gndr.ToString(),
                Gndr => (Gender)Enum.Parse(typeof(Gender), Gndr!));

            builder.Property(a => a.Email)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(a => a.PhoneNumber)
                .HasMaxLength(15)
                .IsRequired();
        }
    }
}
