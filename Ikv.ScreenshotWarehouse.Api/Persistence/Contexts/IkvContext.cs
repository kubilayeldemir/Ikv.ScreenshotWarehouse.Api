using Ikv.ScreenshotWarehouse.Api.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ikv.ScreenshotWarehouse.Api.Persistence.Contexts
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
        public DbSet<PostRawData> PostRawData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne<User>(u => u.User)
                .WithMany(p => p.Posts)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>(entity => { entity.HasIndex(u => u.Username).IsUnique(); });
            modelBuilder.Entity<User>(entity => { entity.HasIndex(u => u.Email).IsUnique(); });
            modelBuilder.Entity<Post>(entity => { entity.HasIndex(p => p.ScreenshotDate); });
            modelBuilder.Entity<Post>(entity => { entity.HasIndex(p => p.CreatedAt); });
            modelBuilder.Entity<Post>(entity => { entity.HasIndex(p => p.IsValidated); });
            modelBuilder.Entity<Post>(entity => { entity.HasIndex(p => p.Md5).IsUnique(); });
            modelBuilder.Entity<Post>(entity => { entity.HasIndex(p => p.TitleMd5); });
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IkvContext).Assembly);
        }
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.LogTo(Console.WriteLine);
        // }
    }
}