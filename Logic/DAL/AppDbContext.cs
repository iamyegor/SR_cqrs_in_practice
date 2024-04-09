using Logic.DAL.Cofigurations.Interfaces;
using Logic.Students;
using Microsoft.EntityFrameworkCore;

namespace Logic.DAL;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Synchronization> Sync => Set<Synchronization>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Username=postgres;Password=yapidr;Database=khorikov_cqrs;Include Error Detail=true"
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IEntitiesConfigurationsAssembly).Assembly
        );
    }
}
