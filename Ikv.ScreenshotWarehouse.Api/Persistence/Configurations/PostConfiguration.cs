using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(p => p.Id)
                .HasMaxLength(64);
            
            builder.Property(p => p.Category)
                .HasMaxLength(64);

            builder.Property(p => p.Title)
                .HasMaxLength(128);

            builder.Property(p => p.GameMap)
                .HasMaxLength(64);

            builder.Property(p => p.GameMap)
                .HasMaxLength(64);

            builder.Property(p => p.FileURL)
                .HasMaxLength(2048);

            builder.Property(p => p.Md5)
                .HasMaxLength(64);

            builder.Property(p => p.Username)
                .HasMaxLength(64);
            
            builder.Property(p => p.GameServer)
                .HasMaxLength(20);
        }
    }
}