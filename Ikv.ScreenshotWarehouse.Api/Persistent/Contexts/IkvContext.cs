using Ikv.ScreenshotWarehouse.Api.Persistent.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ikv.ScreenshotWarehouse.Api.Persistent.Contexts
{
    public class IkvContext : DbContext
    {
        public IkvContext(DbContextOptions<IkvContext> options)
            : base(options)
        {
        }

        public IkvContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne<User>(u => u.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>(entity => { entity.HasIndex(u => u.Username).IsUnique(); });
            modelBuilder.Entity<Post>(entity => { entity.HasIndex(p => p.ScreenshotDate); });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IkvContext).Assembly);
        }
    }
}