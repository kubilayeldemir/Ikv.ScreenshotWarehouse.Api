using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Configurations
{
    public class VideoPostConfiguration : IEntityTypeConfiguration<VideoPost>
    {
        public void Configure(EntityTypeBuilder<VideoPost> builder)
        {
            builder.Property(p => p.Title)
                .HasMaxLength(128);

            builder.Property(p => p.OriginalTitle)
                .HasMaxLength(128);

            builder.Property(p => p.VideoUrl)
                .HasMaxLength(256);
        }
    }
}