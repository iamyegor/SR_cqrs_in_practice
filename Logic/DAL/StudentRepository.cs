using Logic.Students;
using Microsoft.EntityFrameworkCore;

namespace Logic.DAL;

public sealed class StudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public Student? GetById(long id)
    {
        return _context
            .Students.Where(s => s.Id == id)
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Course)
            .FirstOrDefault();
    }

    public IReadOnlyList<Student> GetList(string? enrolledIn, int? numberOfCourses)
    {
        IQueryable<Student> query = _context.Students;

        if (!string.IsNullOrWhiteSpace(enrolledIn))
        {
            query = query.Where(s =>
                s.Enrollments.Count > 0 && s.Enrollments.Any(e => e.Course.Name == enrolledIn)
            );
        }

        if (numberOfCourses != null)
        {
            query = query.Where(s => s.Enrollments.Count == numberOfCourses);
        }

        return query.Include(s => s.Enrollments).ThenInclude(e => e.Course).ToList();
    }

    public void Save(Student student)
    {
        _context.Students.Update(student);
        _context.SaveChanges();
    }

    public void Delete(Student student)
    {
        _context.Students.Where(s => s.Id == student.Id).ExecuteDelete();
    }
}
