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

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Post>().HasKey(p => p.Id);

			builder.Entity<Post>(entity =>
			{
				entity.Property(p => p.PostContent)
					.HasColumnName("Content")
					.HasColumnType("nvarchar(MAX)");

				entity.Property(p => p.Media)
					.HasColumnName("MediaBytes")
					.HasColumnType("nvarchar(MAX)");

				entity.Property(p => p.CreatedDate)
					.HasColumnType("datetime2")
					.HasDefaultValueSql("getdate()");

				entity.Property(p => p.UserId)
					.HasColumnName("user_id")
					.HasColumnType("nvarchar(450)")
					.IsRequired();
			});

			builder.Entity<Post>()
				.HasOne(p => p.User)
				.WithMany(p => p.Posts)
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			builder.Entity<Post>()
				.HasMany(p => p.Comments)
				.WithOne(p => p.Post)
				.HasForeignKey(p => p.PostId);

			builder.Entity<Comments>(entity =>
			{
				entity.Property(c => c.CommentContent)
					.HasColumnName("Content")
					.HasColumnType("nvarchar(MAX)")
					.IsRequired();

				entity.Property(c => c.PostId)
					.HasColumnName("post_id")
					.HasColumnType("int")
					.IsRequired();

				entity.Property(c => c.UserId)
					.HasColumnName("user_id")
					.HasColumnType("nvarchar(450)")
					.IsRequired();

				entity.Property(c => c.CreatedDate)
					.HasColumnName("date")
					.HasColumnType("datetime2")
					.HasDefaultValueSql("getdate()");
			});

			builder.Entity<Comments>()
				.HasOne(c => c.User)
				.WithMany(c => c.Comments)
				.HasForeignKey(c => c.UserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
