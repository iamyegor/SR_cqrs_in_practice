using Logic.Students;
using Microsoft.EntityFrameworkCore;

namespace Logic.DAL.Repositories;

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

    public void Save(Student student)
    {
        _context.Update(student);
        foreach (var removedEnrollment in student.PopRemovedEnrollments())
        {
            _context.Remove(removedEnrollment);
        }

        _context.SaveChanges();
    }

    public void Add(Student student)
    {
        _context.Add(student);
        _context.SaveChanges();
    }

    public void Delete(Student student)
    {
        _context.Students.Where(s => s.Id == student.Id).ExecuteDelete();
    }
}
