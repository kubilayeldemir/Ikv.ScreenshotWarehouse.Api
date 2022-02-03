using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Email)
                .HasMaxLength(320)
                .IsRequired();

            builder.Property(p => p.Email)
                .HasColumnType("CITEXT");
            
            builder.Property(p => p.Email)
                .HasMaxLength(330);

            builder.Property(u => u.Username)
                .HasMaxLength(32)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasMaxLength(int.MaxValue)
                .IsRequired();

            builder.Property(u => u.Salt)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(u => u.Role)
                .HasMaxLength(64);
        }
    }
}