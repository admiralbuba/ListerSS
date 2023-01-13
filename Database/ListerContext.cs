using ListerSS.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ListerSS.Database
{
    public class ListerContext : DbContext
    {
        public ListerContext()
        {
            Database.EnsureCreated();
        }
        public ListerContext(DbContextOptions<ListerContext> options)
            : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=listerSS.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User { Id = Guid.NewGuid(), Name = "qwe" },
                new User { Id = Guid.NewGuid(), Name = "ewq" },
                new User { Id = Guid.NewGuid(), Name = "Katy" },
                new User { Id = Guid.NewGuid(), Name = "Dua" }
                );
        }
    }
}
