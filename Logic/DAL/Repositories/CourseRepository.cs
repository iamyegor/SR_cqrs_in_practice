using Logic.Students;

namespace Logic.DAL.Repositories;

public sealed class CourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public Course? GetByName(string? name)
    {
        if (name == null)
        {
            return null;
        }
        
        return _context.Courses.FirstOrDefault(c => c.Name == name);
    }
}
