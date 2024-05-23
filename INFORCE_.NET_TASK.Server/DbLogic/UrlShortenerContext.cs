using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace INFORCE_.NET_TASK.Server.DbLogic
{
    public class UrlShortenerContext: DbContext
    {
        private string _connectionString;

        public DbSet<ShortenedUrl> Urls { get; set; }
        public DbSet<User> Users { get; set; }
        public UrlShortenerContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>()
                 .HasOne(us => us.User)
                 .WithMany(u => u.Urls)
                 .HasForeignKey(us => us.UserId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_connectionString)
                .LogTo(e => Debug.WriteLine(e));
        }
    }
}
