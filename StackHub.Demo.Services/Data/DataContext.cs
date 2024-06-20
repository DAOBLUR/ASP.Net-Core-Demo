using Microsoft.EntityFrameworkCore;
using StackHub.Demo.Services.Models;

namespace StackHub.Demo.Services.Data
{
    public class DataContext (DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<Administrator>? Administrators { get; set; }
        public DbSet<User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Administrator>().HasIndex(a => a.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }
    }
}