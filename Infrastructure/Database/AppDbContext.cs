using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Template> Templates => Set<Template>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Template>().HasKey(t => t.Id);
            
            modelBuilder.Entity<Template>()
                .Property(t => t.File)
                .HasColumnType("varbinary(max)");
        }
    }
}
