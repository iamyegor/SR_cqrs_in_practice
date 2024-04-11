namespace DbSynchronization.Synchronizers.Students.Models;

public class StudentInJoinQuery
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsCourseDeleted { get; set; } 
    public int NumberOfEnrollments { get; set; }
    public int? CourseGrade { get; set; }
    public string? CourseName { get; set; }
    public int? CourseCredits { get; set; }
}
