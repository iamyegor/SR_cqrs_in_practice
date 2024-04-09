namespace DbSynchronization.Synchronizers.Students.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int NumberOfEnrollments { get; set; }
    public string FirstCourseName { get; set; } = null!;
    public int FirstCourseCredits { get; set; }
    public int FirstCourseGrade { get; set; }
    public string SecondCourseName { get; set; } = null!;
    public int SecondCourseCredits { get; set; }
    public int SecondCourseGrade { get; set; }
}
