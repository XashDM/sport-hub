using Microsoft.EntityFrameworkCore;
using SportHub.Domain.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SportHub.Domain
{
    public class SportHubDBContext : DbContext
    {
        public SportHubDBContext(DbContextOptions options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<NavigationItem> NavigationItems { get; set; }
        public DbSet<DisplayItem> DisplayItems { get; set; }


        public DbSet<Language> Languages { get; set; }
        public DbSet<DisplayedLanguage> DisplayedLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => entity.HasAlternateKey(e => e.Email));
            modelBuilder.Entity<UserRole>(entity => entity.HasAlternateKey(er => er.RoleName));
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<UserRole>()
                .HasIndex(ur => ur.RoleName)
                .IsUnique();
            modelBuilder.Entity<UserRole>()
                .HasData(new UserRole { Id = 1, RoleName = "User" });
            modelBuilder.Entity<UserRole>()
                .HasData(new UserRole { Id = 2, RoleName = "Admin" });
            modelBuilder.Entity<Article>()
                .Property(article => article.IsPublished)
                .HasDefaultValue(false);
        }
    }
}
