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
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=listerSS.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .Property(b => b.Groups)
            //    .Ke();
        }
    }
}
