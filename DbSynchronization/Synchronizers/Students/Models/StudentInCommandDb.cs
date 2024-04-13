namespace DbSynchronization.Synchronizers.Students.Models;

public class StudentInCommandDb
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<EnrollmentInCommandDb> Enrollments { get; set; } = [];
}