using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DnDWebpage.Models;

namespace DnDWebpage.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Characters created by users
        public DbSet<CharacterViewModel> Characters { get; set; }

        public DbSet<Spell> Spells { get; set; }

        // Tavern Bulletin Posts
        public DbSet<BulletinPost> BulletinPosts { get; set; }

        // ✅ Votes cast by users on bulletin posts
        public DbSet<BulletinVote> BulletinVotes { get; set; }

        // ✅ Book Pages for the leather-bound book
        public DbSet<BookPage> BookPages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CharacterViewModel>(entity =>
            {
                entity
                    .HasOne(c => c.User)
                    .WithMany(u => u.Characters)
                    .HasForeignKey(c => c.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Optional: Additional configuration for BookPages (if needed)
            modelBuilder.Entity<BookPage>(entity =>
            {
                entity.HasKey(e => e.PageId);
                entity.Property(e => e.Title).HasMaxLength(200).IsRequired(false);
                entity.Property(e => e.ContentHTML).IsRequired(false);
                entity.Property(e => e.PageNumber).IsRequired();
            });
        }
    }
}
