using E_Commerce.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace E_Commerce.Infrastructure.Data.Configurations
{
    public class UserRefreshTokenConfig : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.Property(u => u.ExpiryDate)
                .IsRequired();

            builder.Property(u => u.IsUsed)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(u => u.IsRevoked)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(u => u.AddedTime)
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(u => u.IsUsed)
                .IsRequired();

            builder.Property(u => u.IsRevoked)
                .IsRequired();

            builder.HasOne(u => u.user)
                .WithMany(u => u.UserRefreshTokens)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
