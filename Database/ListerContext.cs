using Microsoft.EntityFrameworkCore;

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

        //public DbSet<user> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=listerSS.db");
        }
    }
}
