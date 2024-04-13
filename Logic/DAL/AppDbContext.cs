using Logic.DAL.Cofigurations;
using Logic.DAL.Interceptors;
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
        optionsBuilder.AddInterceptors(new SyncInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(IEntityConfigurationsAssembly).Assembly
        );
    }
}

public class Synchronization
{
    public string Name { get; private set; }
    public bool IsSyncRequired { get; set; }
    public int RowVersion { get; private set; }
}
