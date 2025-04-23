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

        // Tavern Bulletin Posts
        public DbSet<BulletinPost> BulletinPosts { get; set; }

        // ✅ Votes cast by users on bulletin posts
        public DbSet<BulletinVote> BulletinVotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CharacterViewModel>(entity =>
            {
                entity
                    .HasOne(c => c.User)
                    .WithMany(u => u.Characters)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // (Optional) future configs for BulletinPost and BulletinVote can go here
        }
    }
}
