using AuthLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthLab.Infra.DbContext;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

public class AuthLabDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseInMemoryDatabase(databaseName: "AuthLabDb");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().Property(x => x.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<User>().Property(x => x.Username).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.Password).IsRequired();
    }
}