#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Models
{
    public class IdenTestDbContext : IdentityDbContext<IdenTestUser>
    {
        public IdenTestDbContext(DbContextOptions<IdenTestDbContext> options): base(options) { }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>().ToTable("Posts");

            modelBuilder.Ignore<IdentityUserLogin<string>>();
        }
    }
}
