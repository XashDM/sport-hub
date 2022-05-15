using Microsoft.EntityFrameworkCore;
using SportHub.Domain.Models;
using System.Linq;

namespace SportHub.Domain
{
    public class SportHubDBContext : DbContext
    {
        public SportHubDBContext(DbContextOptions options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<NavigationItem> NavigationItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
