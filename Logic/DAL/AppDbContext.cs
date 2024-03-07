using Logic.DAL.Cofigurations;
using Logic.Students;
using Microsoft.EntityFrameworkCore;

namespace Logic.DAL;

public class AppDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IEntitiesConfigurationsAssembly).Assembly
        );
    }
}
