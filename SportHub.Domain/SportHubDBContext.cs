using Microsoft.EntityFrameworkCore;
using SportHub.Domain.Models;

namespace SportHub.Domain
{
    public class SportHubDBContext : DbContext
    {
        public SportHubDBContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

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
