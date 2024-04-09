namespace DbSynchronization.Synchronizers.Students.Models;

public class StudentInCommandDb
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int NumberOfEnrollments { get; set; }
    public int Grade { get; set; }
    public string CourseName { get; set; } = null!;
    public int CourseCredits { get; set; }
}
