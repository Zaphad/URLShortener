using Microsoft.EntityFrameworkCore;

namespace URLShortener.Models
{
    public class URLLinkContext : DbContext
    {  
        public URLLinkContext(DbContextOptions<URLLinkContext> options) : base(options) {}
        public DbSet<URLLink> URLLinks { get; set; }
        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);
            builder.Entity<URLLink>().ToTable("URLLink");
        }
    }
}
